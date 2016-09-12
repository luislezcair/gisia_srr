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
        public static void Perform(int projectId, string userName)
        {
            QuestionDBContext db = new QuestionDBContext();

            var project = db.Projects.Find(projectId);

            project.State = ProjectState.WORKING;
            project.StateTime = DateTime.Now;

            var searchKeys = SearchKeyGenerator.BuildSearchKey(project.Answers.ToList(), project.Language);

            // Guardamos las claves de búsqueda asociadas al proyecto
            project.SearchKeys.Clear();
            foreach(var key in searchKeys)
            {
                project.SearchKeys.Add(new ProjectSearchKey {
                    Project = project,
                    KeyString = key
                });
            }

            db.SaveChanges();

            try
            {
                GisiaClient webservice = new GisiaClient(userName, project);
                webservice.SendRequest(searchKeys);

                // Se inició el proceso de búsqueda. Establecer como activo este proyecto
                project.State = ProjectState.ACTIVE;
                project.StateTime = DateTime.Now;
                db.SaveChanges();
            }
            catch(AggregateException e)
            {
                var ex = e.InnerException;

                project.State = ProjectState.ERROR;
                project.StateTime = DateTime.Now;
                project.StateReason = ex.Message;

                db.SaveChanges();
            }
        }
    }
}