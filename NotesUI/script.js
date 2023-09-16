

const savebutton = document.querySelector("#buttonSave");

const title = document.querySelector("#title");

const description = document.querySelector("#description");

const container = document.querySelector("#container");
const delbutton = document.querySelector("#buttonDelete");

function addNote( title, description){


    const body ={ 
        title : title,
        description : description,
        isVisible:true

    };

fetch('https://localhost:7159/api/notes',{
    method: 'POST',
    body: JSON.stringify(body),
    headers:{
        "content-type" : "application/json"
    }
})
.then(data => data.json())
.then(response => {
    clearForm();
    getAllNotes();
});
}





function updateNote(id , title, description){


    const body ={ 
        title : title,
        description : description,
        isVisible:true

    };

fetch(`https://localhost:7159/api/notes/${id}`,{
    method: 'PUT',
    body: JSON.stringify(body),
    headers:{
        "content-type" : "application/json"
    }
})
.then(data => data.json())
.then(response => {
    clearForm();
    getAllNotes();
});
}



function deleteNote(id)
{

fetch(`https://localhost:7159/api/notes/${id}`,{
    method: 'DELETE',
    headers:{
        "content-type" : "application/json"
    }
})
.then(response => {
clearForm();
getAllNotes();
});
}

function clearForm(){
    title.value = '';
    description.value = '';
    delbutton.classList.add('hidden');
}


function populateForm(id)
{
   getNotebyID(id);
}

function getNotebyID(id){
    fetch(`https://localhost:7159/api/notes/${id}`)
    .then(data => data.json())
    .then(res => displayNotesinForm(res));
    // .then(res => console.log(res));
}
    
function displayNotesinForm(note){
   title.value = note.title;
   description.value = note.description;
   delbutton.classList.remove('hidden');
   delbutton.setAttribute('data-id', note.id);
   savebutton.setAttribute('data-id',note.id);
}


function displayNotes(notes){
 let allNotes ='';


notes.forEach(note => {
  const noteElement =   `
                        <div class="note" data-id="${note.id}">
                        <h3>${note.title}</h3>
                        <p>${note.description}</p>
                        </div>
                        `;
                        allNotes += noteElement;

});
container.innerHTML = allNotes;


//add evenistener

document.querySelectorAll('.note').forEach(note => {
    note.addEventListener('click', function(){
        populateForm(note.dataset.id)

    });
});


}

function getAllNotes(){
    fetch('https://localhost:7159/GetAllNotes')
    .then(data => data.json())
    .then(res => displayNotes(res));
    // .then(res => console.log(res));
}
    
getAllNotes();

savebutton.addEventListener('click', function(){

    const id = savebutton.dataset.id;
    if(id)
    {
        updateNote(id,title.value, description.value);
    }
    else{

        addNote(title.value, description.value);
    }
   
});



delbutton.addEventListener('click',function(){
   const id =  delbutton.dataset.id ;
    deleteNote(id);
})

