using AppNotes.Models;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Services
{
    public class LibraryService(ConexionBBDD _conn)
    {
        public async Task<string> CreateNote(string token, string FirebasePath, Note note)
        {
            if (!token.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                    var notefirebase = await client.Child("note").PostAsync(note);
                    note.Id = notefirebase.Key;
                    note.User = userId;
                    await client.Child("note").Child(notefirebase.Key).PutAsync(note);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = note.Id,
                        Type = DocumentType.Note
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(note);
            return note.Id;
        }
        public async Task<string> CreateNotebook(string token, string FirebasePath, Notebook notebook)
        {
            if (!token.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                    var notefirebookbase = await client.Child("notebook").PostAsync(notebook);
                    notebook.Id = notefirebookbase.Key;
                    notebook.User = userId;
                    await client.Child("notebook").Child(notefirebookbase.Key).PutAsync(notebook);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = notebook.Id,
                        Type = DocumentType.Notebook
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(notebook);
            return notebook.Id;
        }
        public async void DeleteNote(string token, string FirebasePath, Note note)
        {
            var notesinnotebook = _conn.GetNotes().Where(x => x.Notebook.Equals(note.Notebook) && !x.Id.Equals(note.Id)).OrderBy(x => x.Position).ToList();
            var num = 0;
            foreach (var notetoposition in notesinnotebook)
            {
                notetoposition.Position = num;
                notetoposition.Modified = DateTime.UtcNow;
                _conn.Conn.Update(notetoposition);
                num++;
            }

            if (!string.IsNullOrEmpty(note.Notebook))
            {
                var notebook = _conn.GetNotebook(note.Notebook);
                if (notebook.Bookmark.Equals(note.Id))
                {
                    notebook.Bookmark = "";
                    _conn.Conn.Update(notebook);
                }
            }

            if (!token.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Note && x.Id.Equals(note.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("note").Child(note.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = note.Id,
                        Type = DocumentType.Note
                    };
                    _conn.GetConnection().Insert(item);
                }
            }
            _conn.GetConnection().Delete(note);
        }
        public async void DeleteNotebook(string token, string FirebasePath, Notebook notebook)
        {
            var notes = _conn.GetNotes().Where(x => x.Notebook.Equals(notebook.Id)).ToList();
            foreach (var note in notes)
            {
                note.Modified = DateTime.UtcNow;
                note.Notebook = "";
                note.Position = -1;
                _conn.GetConnection().Update(note);
            }

            if (!token.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Notebook && x.Id.Equals(notebook.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("notebook").Child(notebook.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = notebook.Id,
                        Type = DocumentType.Notebook
                    };
                    _conn.GetConnection().Insert(item);
                }
            }
            _conn.GetConnection().Delete(notebook);
        }

    }
}
