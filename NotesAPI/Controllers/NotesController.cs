using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Data;
using NotesAPI.Models.Entities;
using System.Data;

namespace NotesAPI.Controllers
{
    [ApiController] //annotate this is an api controller no views
    [Route("api/[controller]")]

    public class NotesController : Controller
    {
        private readonly NotesDbContext notesDbContext;

        public NotesController(NotesDbContext notesDbContext)
        {
            this.notesDbContext = notesDbContext;
        }

        [HttpGet]
        [Route("/GetAllNotes")]
        public async Task<IActionResult> GetAllNotes() // 
        {
          
            return Ok(await notesDbContext.Notes.ToListAsync()); //call Notes table , property define in dbset


            //Get notes from the databse , use notesdbcontext  which we injected in services
        }

        //public IActionResult GetAllNotes() // 
        //{
        //    //Get notes from the databse , use notesdbcontext  which we injected in services
        //    return Ok(notesDbContext.Notes); //call Notes table , property define in dbset
        //}


        [HttpGet]
        [Route("GetAllNotes1")]
        public async Task<IActionResult> GetAllNotes1() // 
        {
            //Get notes from the databse , use notesdbcontext  which we injected in services
            return Ok(await notesDbContext.Notes.ToListAsync()); //call Notes table , property define in dbset
        }


        [HttpGet]
        [Route("{id:Guid}")]
        [ActionName("GetNotebyId")]
        public async Task<IActionResult> GetNotebyId([FromRoute] Guid id)
        {
            //await notesDbContext.Notes.FirstOrDefaultAsync(x => x.Id == id);
            //or
            var note = await notesDbContext.Notes.FindAsync(id);///tofind notes with id 
            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            await notesDbContext.Notes.AddAsync(note);
            await notesDbContext.SaveChangesAsync(); // to persisit the data
            return CreatedAtAction(nameof(GetNotebyId), new { id = note.Id }, note);
            //return created response action name , pass complete location of newly created resource 
            ///in the header of response u will see location to newly created resource
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note updatenote)
        {
            var exisitingNote = await notesDbContext.Notes.FindAsync(id);///tofind notes with id 
            if (exisitingNote == null)
            {
                return NotFound();
            }
            exisitingNote.Title = updatenote.Title;
            exisitingNote.Description = updatenote.Description;
            exisitingNote.IsVisibile = updatenote.IsVisibile;

            await notesDbContext.SaveChangesAsync();

            return Ok(exisitingNote);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var exisitingNote = await notesDbContext.Notes.FindAsync(id);
            if (exisitingNote == null)
            {
                return NotFound();
            }
            notesDbContext.Notes.Remove(exisitingNote);
            await notesDbContext.SaveChangesAsync();
            return Ok();

        }






    }
}
