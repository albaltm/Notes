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
        private string FirebasePath = "https://appnotes-d61d3-default-rtdb.firebaseio.com/";
        
        public async Task TrySync(string token)
        {
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                client.Dispose();
                await TrySyncLibrary(token);
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
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                client.Dispose();

                await SyncDeleteQueueLibrary();
                await SyncCreateQueueLibrary(userId);

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
        private async Task SyncDeleteQueueLibrary()
        {
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                
                var deletequeue = _conn.GetDeleteQueue();
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
                client.Dispose();
            }
            catch { }
        }
        private async Task SyncCreateQueueLibrary(string userId)
        {
            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var createqueue = _conn.GetCreateQueue();
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
                client.Dispose();
            }
            catch { }
        }
    }
}
