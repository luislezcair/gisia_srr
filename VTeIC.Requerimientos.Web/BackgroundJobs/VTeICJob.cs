using Hangfire;
using System;
using System.Linq;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.SearchKey;
using VTeIC.Requerimientos.Web.WebService;

namespace VTeIC.Requerimientos.Web.BackgroundJobs
{
    public class VTeICJob
    {
        [AutomaticRetry(Attempts = 2, LogEvents = true)]
        public static void Perform(int projectId, string userName)
        {
            QuestionDBContext db = new QuestionDBContext();

            var project = db.Projects.Find(projectId);

            project.State = ProjectState.WORKING;
            project.StateTime = DateTime.Now;

            // Si ya se generaron claves de búsqueda para este proyecto, no las generamos de nuevo.
            // Puede pasar que ya se hayan generado claves si estamos en otro attempt del job.
            if(!project.SearchKeys.Any())
            {
                var searchKeys = SearchKeyGenerator.BuildSearchKey(project.Answers.ToList(), project.Language);

                // Guardamos las claves de búsqueda generadas
                foreach (var key in searchKeys)
                {
                    project.SearchKeys.Add(new ProjectSearchKey
                    {
                        Project = project,
                        KeyString = key
                    });
                }
            }

            db.SaveChanges();

            try
            {
                GisiaClient webservice = new GisiaClient(userName, project);
                webservice.SendRequest();

                // Se inició el proceso de búsqueda. Establecer como activo este proyecto
                project.State = ProjectState.ACTIVE;
                project.StateTime = DateTime.Now;
                db.SaveChanges();
            }
            catch(AggregateException e)
            {
                var ex = e.InnerException;

                if (ex.InnerException != null)
                    ex = ex.InnerException;

                project.State = ProjectState.ERROR;
                project.StateTime = DateTime.Now;
                project.StateReason = ex.Message;

                db.SaveChanges();

                // Debería hacer que el Job falle y se reintente nuevamente después
                throw ex;
            }
        }
    }
}