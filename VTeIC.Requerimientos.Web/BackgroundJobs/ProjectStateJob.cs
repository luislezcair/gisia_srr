using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.WebService;

namespace VTeIC.Requerimientos.Web.BackgroundJobs
{
    public class ProjectStateJob
    {
        public static void RequestProjectState(int projectId, string userName)
        {
            QuestionDBContext db = new QuestionDBContext();
            var project = db.Projects.Find(projectId);

            if (project == null)
                return;

            try
            {
                var client = new GisiaClient(userName, project);
                var state = client.GetProjectStatus();

                if(state != null)
                {
                    project.WSState = state.estado;
                    project.WSStopped = state.stop;
                    project.StateTime = DateTime.Now;

                    if(state.stop)
                    {
                        project.State = Entidades.ProjectState.FINISHED;
                    }
                    else
                    {
                        project.State = Entidades.ProjectState.ACTIVE;
                    }
                }
                else
                {
                    project.State = Entidades.ProjectState.ERROR;
                    project.StateReason = "El proyecto no existe en el servicio de minería de datos";
                }

                db.SaveChanges();
            }
            catch (AggregateException e)
            {
                var ex = e.InnerException;

                if (ex.InnerException != null)
                    ex = ex.InnerException;

                throw;
            }
        }
    }
}