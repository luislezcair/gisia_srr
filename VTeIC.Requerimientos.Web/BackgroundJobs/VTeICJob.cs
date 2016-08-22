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
            db.SaveChanges();

            Session session = db.Sessions.OrderByDescending(s => s.Id).First();
            var searchKeys = SearchKeyGenerator.BuildSearchKey(session.Answers, project.Language);

            try
            {
                GisiaClient webservice = new GisiaClient(userName, project);
                webservice.SendRequest(searchKeys);

                // Se inició el proceso de búsqueda. Establecer como activo este proyecto
                project.State = ProjectState.ACTIVE;
                db.SaveChanges();
            }
            catch(AggregateException e)
            {
                var ex = e.InnerException;

                project.State = ProjectState.ERROR;
                project.StateReason = ex.Message;

                db.SaveChanges();
            }
        }
    }
}