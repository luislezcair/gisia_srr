using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VTeIC.Requerimientos.Entidades;
using VTeIC.Requerimientos.Web.Models;
using VTeIC.Requerimientos.Web.ViewModels;

namespace VTeIC.Requerimientos.Web.Controllers
{
    public class QuestionRestController : ApiController
    {
        private readonly QuestionDBContext _db = new QuestionDBContext();
        private readonly QuestionManager _manager = new QuestionManager();

        // GET: api/QuestionRest
        public IEnumerable<QuestionViewModel> GetQuestions()
        {
            var questionViews = new List<QuestionViewModel>();
            var questions = _manager.QuestionList();

            foreach (var question in questions)
            {
                questionViews.Add(new QuestionViewModel(question));
            }

            return questionViews;
        }
/*
        [Route("api/QuestionRest/first")]
        [ResponseType(typeof(QuestionViewModel))]
        public IHttpActionResult GetFirstQuestion()
        {
            return Ok(new QuestionViewModel(_manager.FirstQuestion()));
        }

        [Route("api/QuestionRest/links")]
        public IEnumerable<QuestionLinkViewModel> GetLinks()
        {
            var questionLinkViews = new List<QuestionLinkViewModel>();
            //var questionLinks = _db.Set<QuestionLink>();
            var questionLinks = _db.QuestionLinks.ToList();

            foreach (var questionLink in questionLinks)
            {
                questionLinkViews.Add(new QuestionLinkViewModel(questionLink));
            }

            return questionLinkViews;
        }
*/
        // GET: api/QuestionRest/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult GetQuestion(int id)
        {
            Question question = _db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(new QuestionViewModel(question));
        }

        // PUT: api/QuestionRest/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest();
            }

            _db.Entry(question).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/QuestionRest
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Questions.Add(question);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        // DELETE: api/QuestionRest/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id)
        {
            Question question = _db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            _db.Questions.Remove(question);
            _db.SaveChanges();

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return _db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}