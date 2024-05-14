using AppNotes.Components.Pages;
using AppNotes.Models;
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

namespace AppNotes.Services
{
    public class SynchronizationService(IJSRuntime jSRuntime, ConexionBBDD _conn)
    {
        private string FirebasePath = "https://appnotes-d61d3-default-rtdb.firebaseio.com/";
        public event Action? OnSynchronizationChanged;

        private bool _synchronized = true;
        public bool Synchronized
        {
            get => _synchronized;
            set
            {
                _synchronized = value;
                //OnSynchronizationChanged?.Invoke();
            }
        }
        public async void TrySync()
        {
            //_conn.GetConnection().DeleteAll<Note>();
            var token = jSRuntime.InvokeAsync<string>("localStorage.getItem", "token");
            if (token.Equals("guest"))
            {
                Synchronized = false;
                return;
            }

            try
            {
                FirebaseClient client = new FirebaseClient(FirebasePath);
                var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;

                //para eliminar
                var deletequeue = _conn.GetDeleteQueue();
                foreach (var item in deletequeue)
                {
                    if (item.Type == DocumentType.Note)
                    {
                        await client.Child("note").Child(item.Id).DeleteAsync();
                    }
                    _conn.Conn.Delete(item);
                }

                //para actualizar
                var notes = _conn.GetNotesByUser(userId);
                foreach (var note in notes)
                {
                    if (!note.Synchronized)
                    {
                        //si no esta subida
                        var existentNote = (await client.Child("note").OnceAsync<Note>()).Select(x => x.Object).Where(x => x.Id.Equals(note.Id)).ToList().FirstOrDefault();
                        if (existentNote == null) 
                        {
                            var firebaseNote = await client.Child("note").Child(note.Id).PostAsync(note);
                            note.Id = firebaseNote.Key;
                        }
                        note.Synchronized = true;
                        await client.Child("note").Child(note.Id).PutAsync(note);
                    }
                }
                //darle una vuelta a esto

                

                client.Dispose();
            }
            catch { Synchronized = false; return; }

            Synchronized = true;
        }
    }
}
