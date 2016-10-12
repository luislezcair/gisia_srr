using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.WebService;

namespace VTeIC.Requerimientos.Web.BackgroundJobs
{
    public class ProjectStopJob
    {
        public static void SendProjectStop(int projectId, string userName)
        {
            QuestionDBContext db = new QuestionDBContext();
            var project = db.Projects.Find(projectId);

            try
            {
                var client = new GisiaClient(userName, project);
                client.SendStopSignal();

                project.State = Entidades.ProjectState.FINISHED;
                project.StateReason = "El proyecto ha sido detenido por el usuario";

                db.SaveChanges();
            }
            catch (AggregateException e)
            {
                var ex = e.InnerException;

                if (ex.InnerException != null)
                    ex = ex.InnerException;

                Debug.Print("ERRORRRRR!!!!!11");
                Debug.Print(ex.Message);

                throw;
            }
        }
    }
}