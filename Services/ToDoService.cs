using AppNotes.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Services
{
    public class ToDoService(ConexionBBDD _conn)
    {
        public async Task<string> CreateToDo(string token, string FirebasePath, ToDoItem todo)
        {
            if (!token.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                    var todofirebase = await client.Child("todo").PostAsync(todo);
                    todo.Id = todofirebase.Key;
                    todo.User = userId;

                    await client.Child("todo").Child(todofirebase.Key).PutAsync(todo);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = todo.Id,
                        Type = DocumentType.Todo
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(todo);
            return todo.Id;
        }
        public async Task DeleteToDo(string token, string FirebasePath, ToDoItem todo)
        {
            var subtodos = _conn.GetSubToDos().Where(x => x.ToDo.Equals(todo.Id));
            foreach (var subtodo in subtodos)
            {
                await DeleteSubToDo(token, FirebasePath, subtodo);
            }
            if (!token.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Todo && x.Id.Equals(todo.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("todo").Child(todo.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = todo.Id,
                        Type = DocumentType.Todo
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(todo);
        }

        public async Task<string> CreateSubToDo(string token, string FirebasePath, SubToDo subtodo)
        {
            if (!token.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                    var subtodofirebase = await client.Child("subtodo").PostAsync(subtodo);
                    subtodo.Id = subtodofirebase.Key;
                    subtodo.User = userId;

                    await client.Child("subtodo").Child(subtodofirebase.Key).PutAsync(subtodo);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = subtodo.Id,
                        Type = DocumentType.SubToDo
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(subtodo);
            return subtodo.Id;
        }
        public async Task DeleteSubToDo(string token, string FirebasePath, SubToDo subtodo)
        {
            if (!token.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.SubToDo && x.Id.Equals(subtodo.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("subtodo").Child(subtodo.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = subtodo.Id,
                        Type = DocumentType.SubToDo
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(subtodo);
        }
    }
}
