using AppNotes.Components.Pages;
using AppNotes.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.JSInterop;
using Microsoft.Maui.ApplicationModel.Communication;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace AppNotes.Services
{
    public class SynchronizationService(ConexionBBDD _conn)
    {
        private readonly string FirebasePath = "https://appnotes-d61d3-default-rtdb.firebaseio.com/";
        
        public async Task TrySync(string token)
        {
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                client.Dispose();
                
                await TrySyncLibrary(token);
                await TrySyncEvents(token);
                await TrySyncToDo(token);
                await TrySyncRoutines(token);
            }
            catch { }
        }

        public async Task TrySyncLibrary(string token)
        {
            if (token.Equals("guest"))
            {
                return;
            }
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().First().User;

                await SyncCreateQueueLibrary(client, userId);
                await SyncDeleteQueueLibrary(client);
                
                client.Dispose();

                var notesFirebase = (await client.Child("note").OnceAsync<Note>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var notes = _conn.GetNotes();
                foreach (var noteFirebase in notesFirebase)
                {
                    Note? note = _conn.GetNote(noteFirebase.Id);
                    if (note == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(noteFirebase);
                    }
                    else
                    {
                        notes.Remove(notes.First(x => x.Id == note.Id));
                        if (noteFirebase.Modified != note.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (noteFirebase.Modified > note.Modified)
                            {
                                _conn.Conn.Update(noteFirebase);
                            }
                            else
                            {
                                await client.Child("note").Child(note.Id).PutAsync(note);
                            }
                        }
                    }

                }

                var notebooksFirebase = (await client.Child("notebook").OnceAsync<Notebook>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var notebooks = _conn.GetNotebooks();
                foreach (var notebookFirebase in notebooksFirebase)
                {
                    Notebook? notebook = _conn.GetNotebook(notebookFirebase.Id);
                    if (notebook == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(notebookFirebase);
                    }
                    else
                    {
                        var toremove = notebooks.First(u => u.Id == notebook.Id);
                        notebooks.Remove(toremove);
                        if (notebookFirebase.Modified != notebook.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (notebookFirebase.Modified > notebook.Modified)
                            {
                                _conn.Conn.Update(notebookFirebase);
                            }
                            else
                            {
                                await client.Child("notebook").Child(notebook.Id).PutAsync(notebook);
                            }
                        }
                    }

                }

                //si algo se ha eliminado en otro dispositivo
                foreach (var note in notes)
                {
                    _conn.Conn.Delete(note);
                }
                foreach (var notebook in notebooks)
                {
                    _conn.Conn.Delete(notebook);
                }
            }
            catch { }
            
        }
        public async Task TrySyncEvents(string token)
        {
            if (token.Equals("guest"))
            {
                return;
            }
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().First().User;
                
                await SyncCreateQueueEvents(client,userId);
                await SyncDeleteQueueEvents(client);

                client.Dispose();

                var eventsFirebase = (await client.Child("event").OnceAsync<Event>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var events = _conn.GetEvents();
                foreach (var eventFirebase in eventsFirebase)
                {
                    Event? evento = _conn.GetEvent(eventFirebase.Id);
                    if (evento == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(eventFirebase);
                    }
                    else
                    {
                        events.Remove(events.First(x => x.Id == evento.Id));
                        if (eventFirebase.Modified != evento.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (eventFirebase.Modified > evento.Modified)
                            {
                                _conn.Conn.Update(eventFirebase);
                            }
                            else
                            {
                                await client.Child("event").Child(evento.Id).PutAsync(evento);
                            }
                        }
                    }

                }

                //si algo se ha eliminado en otro dispositivo
                foreach (var evento in events)
                {
                    _conn.Conn.Delete(evento);
                }
            }
            catch { }
            
        }
        public async Task TrySyncToDo(string token)
        {
            if (token.Equals("guest"))
            {
                return;
            }
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().First().User;

                await SyncCreateQueueToDo(client, userId);
                await SyncDeleteQueueToDo(client);

                client.Dispose();

                var subtodosFirebase = (await client.Child("subtodo").OnceAsync<SubToDo>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var subtodos = _conn.GetSubToDos();
                foreach (var subtodoFirebase in subtodosFirebase)
                {
                    SubToDo? subtodo = _conn.GetSubToDo(subtodoFirebase.Id);
                    if (subtodo == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(subtodoFirebase);
                    }
                    else
                    {
                        subtodos.Remove(subtodos.First(x => x.Id == subtodo.Id));
                        if (subtodoFirebase.Modified != subtodo.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (subtodoFirebase.Modified > subtodo.Modified)
                            {
                                _conn.Conn.Update(subtodoFirebase);
                            }
                            else
                            {
                                await client.Child("subtodo").Child(subtodo.Id).PutAsync(subtodo);
                            }
                        }
                    }

                }

                var todosFirebase = (await client.Child("todo").OnceAsync<ToDoItem>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var todos = _conn.GetToDos();
                foreach (var todoFirebase in todosFirebase)
                {
                    ToDoItem? todo = _conn.GetToDo(todoFirebase.Id);
                    if (todo == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(todoFirebase);
                    }
                    else
                    {
                        var toremove = todos.First(u => u.Id == todo.Id);
                        todos.Remove(toremove);
                        if (todoFirebase.Modified != todo.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (todoFirebase.Modified > todo.Modified)
                            {
                                _conn.Conn.Update(todoFirebase);
                            }
                            else
                            {
                                await client.Child("todo").Child(todo.Id).PutAsync(todo);
                            }
                        }
                    }

                }

                //si algo se ha eliminado en otro dispositivo
                foreach (var subtodo in subtodos)
                {
                    _conn.Conn.Delete(subtodo);
                }
                foreach (var todo in todos)
                {
                    _conn.Conn.Delete(todo);
                }
            }
            catch { }
        }
        public async Task TrySyncRoutines(string token)
        {
            if (token.Equals("guest"))
            {
                return;
            }
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().First().User;

                await SyncCreateQueueRoutine(client, userId);
                await SyncDeleteQueueRoutine(client);

                client.Dispose();

                var registriesFirebase = (await client.Child("routineregistry").OnceAsync<RoutineRegistry>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var registries = _conn.GetRoutinesRegistry();
                foreach (var registryFirebase in registriesFirebase)
                {
                    RoutineRegistry? registry = _conn.GetRoutineRegistry(registryFirebase.Id);
                    if (registry == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(registryFirebase);
                    }
                    else
                    {
                        registries.Remove(registries.First(x => x.Id == registry.Id));
                        if (registryFirebase.Modified != registry.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (registryFirebase.Modified > registry.Modified)
                            {
                                _conn.Conn.Update(registryFirebase);
                            }
                            else
                            {
                                await client.Child("routineregistry").Child(registry.Id).PutAsync(registry);
                            }
                        }
                    }

                }

                var routinesFirebase = (await client.Child("routine").OnceAsync<Routine>()).Select(x => x.Object).Where(x => x.User.Equals(userId)).ToList();
                var routines = _conn.GetRoutines();
                foreach (var routineFirebase in routinesFirebase)
                {
                    Routine? routine = _conn.GetRoutine(routineFirebase.Id);
                    if (routine == null)
                    {
                        //se ha creado en otro dispositivo
                        _conn.Conn.Insert(routineFirebase);
                    }
                    else
                    {
                        var toremove = routines.First(u => u.Id == routine.Id);
                        routines.Remove(toremove);
                        if (routineFirebase.Modified != routine.Modified)
                        {
                            //la ultima modificacion no concuerda
                            if (routineFirebase.Modified > routine.Modified)
                            {
                                _conn.Conn.Update(routineFirebase);
                            }
                            else
                            {
                                await client.Child("routine").Child(routine.Id).PutAsync(routine);
                            }
                        }
                    }

                }

                //si algo se ha eliminado en otro dispositivo
                foreach (var registry in registries)
                {
                    _conn.Conn.Delete(registry);
                }
                foreach (var routine in routines)
                {
                    _conn.Conn.Delete(routine);
                }
            }
            catch { }
        }
        private async Task SyncDeleteQueueLibrary(FirebaseClient client)
        {
            try
            {
                var deletequeue = _conn.GetDeleteQueue().Where(x => x.Type == DocumentType.Note || x.Type == DocumentType.Notebook);
                foreach (var item in deletequeue)
                {
                    if (item.Type == DocumentType.Note)
                    {
                        await client.Child("note").Child(item.Id).DeleteAsync();
                    }
                    else if (item.Type == DocumentType.Notebook)
                    {
                        await client.Child("notebook").Child(item.Id).DeleteAsync();
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncDeleteQueueEvents(FirebaseClient client)
        {
            try
            {
                var deletequeue = _conn.GetDeleteQueue().Where(x => x.Type == DocumentType.Event);
                foreach (var item in deletequeue)
                {
                    await client.Child("event").Child(item.Id).DeleteAsync();
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncDeleteQueueToDo(FirebaseClient client)
        {
            try
            {
                var deletequeue = _conn.GetDeleteQueue().Where(x => x.Type == DocumentType.SubToDo || x.Type == DocumentType.Todo);
                foreach (var item in deletequeue)
                {
                    if (item.Type == DocumentType.SubToDo)
                    {
                        await client.Child("subtodo").Child(item.Id).DeleteAsync();
                    }
                    else if (item.Type == DocumentType.Todo)
                    {
                        await client.Child("todo").Child(item.Id).DeleteAsync();
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncDeleteQueueRoutine(FirebaseClient client)
        {
            try
            {
                var deletequeue = _conn.GetDeleteQueue().Where(x => x.Type == DocumentType.RoutineRegistry || x.Type == DocumentType.Routine);
                foreach (var item in deletequeue)
                {
                    if (item.Type == DocumentType.RoutineRegistry)
                    {
                        await client.Child("routineregistry").Child(item.Id).DeleteAsync();
                    }
                    else if (item.Type == DocumentType.Routine)
                    {
                        await client.Child("routine").Child(item.Id).DeleteAsync();
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncCreateQueueLibrary(FirebaseClient client, string userId)
        {
            try
            {
                var createqueue = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Note || x.Type == DocumentType.Notebook);
                foreach (var item in createqueue)
                {
                    if (item.Type == DocumentType.Note)
                    {
                        var note = _conn.GetNote(item.Id);
                        _conn.Conn.Delete(note);
                        var firebaseItem = await client.Child("note").PostAsync(note);
                        note.Id = firebaseItem.Key;
                        note.User = userId;
                        await client.Child("note").Child(note.Id).PutAsync(note);
                        _conn.Conn.Insert(note);
                    }
                    else if (item.Type == DocumentType.Notebook)
                    {
                        var notebook = _conn.GetNotebook(item.Id);
                        _conn.Conn.Delete(notebook);
                        var firebaseItem = await client.Child("notebook").PostAsync(notebook);
                        notebook.Id = firebaseItem.Key;
                        notebook.User = userId;
                        await client.Child("notebook").Child(notebook.Id).PutAsync(notebook);
                        _conn.Conn.Insert(notebook);
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncCreateQueueEvents(FirebaseClient client, string userId)
        {
            try
            {
                var createqueue = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Event);
                foreach (var item in createqueue)
                {
                    var evento = _conn.GetEvent(item.Id);
                    _conn.Conn.Delete(evento);
                    evento.User = userId;
                    var firebaseItem = await client.Child("event").PostAsync(evento);
                    evento.Id = firebaseItem.Key;
                    await client.Child("event").Child(evento.Id).PutAsync(evento);
                    _conn.Conn.Insert(evento);

                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncCreateQueueToDo(FirebaseClient client, string userId)
        {
            try
            {
                var createqueue = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.SubToDo || x.Type == DocumentType.Todo);
                foreach (var item in createqueue)
                {
                    if (item.Type == DocumentType.SubToDo)
                    {
                        var subtodo = _conn.GetSubToDo(item.Id);
                        _conn.Conn.Delete(subtodo);
                        var firebaseItem = await client.Child("subtodo").PostAsync(subtodo);
                        subtodo.Id = firebaseItem.Key;
                        subtodo.User = userId;
                        await client.Child("subtodo").Child(subtodo.Id).PutAsync(subtodo);
                        _conn.Conn.Insert(subtodo);
                    }
                    else if (item.Type == DocumentType.Todo)
                    {
                        var todo = _conn.GetToDo(item.Id);
                        _conn.Conn.Delete(todo);
                        var firebaseItem = await client.Child("todo").PostAsync(todo);
                        todo.Id = firebaseItem.Key;
                        todo.User = userId;
                        await client.Child("todo").Child(todo.Id).PutAsync(todo);
                        _conn.Conn.Insert(todo);
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
        private async Task SyncCreateQueueRoutine(FirebaseClient client, string userId)
        {
            try
            {
                var createqueue = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.RoutineRegistry || x.Type == DocumentType.Routine);
                foreach (var item in createqueue)
                {
                    if (item.Type == DocumentType.RoutineRegistry)
                    {
                        var registry = _conn.GetRoutineRegistry(item.Id);
                        _conn.Conn.Delete(registry);
                        var firebaseItem = await client.Child("routineregistry").PostAsync(registry);
                        registry.Id = firebaseItem.Key;
                        registry.User = userId;
                        await client.Child("routineregistry").Child(registry.Id).PutAsync(registry);
                        _conn.Conn.Insert(registry);
                    }
                    else if (item.Type == DocumentType.Routine)
                    {
                        var routine = _conn.GetRoutine(item.Id);
                        _conn.Conn.Delete(routine);
                        var firebaseItem = await client.Child("routine").PostAsync(routine);
                        routine.Id = firebaseItem.Key;
                        routine.User = userId;
                        await client.Child("routine").Child(routine.Id).PutAsync(routine);
                        _conn.Conn.Insert(routine);
                    }
                    _conn.Conn.Delete(item);
                }
            }
            catch { }
        }
    }
}
