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
        public async Task<string> CreateToDo(string user, string FirebasePath, ToDoItem todo)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var todofirebase = await client.Child("todo").PostAsync(todo);
                    todo.Id = todofirebase.Key;
                    todo.User = user;

                    await client.Child("todo").Child(todofirebase.Key).PutAsync(todo);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = todo.Id,
                        Type = DocumentType.Todo,
                        User = user,
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(todo);
            return todo.Id;
        }
        public async Task DeleteToDo(string user, string FirebasePath, ToDoItem todo)
        {
            var subtodos = _conn.GetSubToDos(user).Where(x => x.ToDo.Equals(todo.Id));
            foreach (var subtodo in subtodos)
            {
                await DeleteSubToDo(user, FirebasePath, subtodo);
            }
            if (!user.Equals("guest"))
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
                        Type = DocumentType.Todo,
                        User = user,
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(todo);
        }

        public async Task<string> CreateRoutine(string user, string FirebasePath, Routine routine)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var routinefirebase = await client.Child("routine").PostAsync(routine);
                    routine.Id = routinefirebase.Key;
                    routine.User = user;

                    await client.Child("routine").Child(routinefirebase.Key).PutAsync(routine);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = routine.Id,
                        Type = DocumentType.Routine,
                        User = user
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(routine);
            return routine.Id;
        }
        public async Task DeleteRoutine(string user, string FirebasePath, Routine routine)
        {
            var routineRegistry = _conn.GetRoutinesRegistry(user).Where(x =>x.Routine.Equals(routine.Id));
            foreach (var registry in routineRegistry)
            {
                await DeleteRoutineRegistry(user, FirebasePath, registry);
            }
            if (!user.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Routine && x.Id.Equals(routine.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("routine").Child(routine.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = routine.Id,
                        Type = DocumentType.Routine,
                        User = user,
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(routine);
        }

        public async Task<string> CreateSubToDo(string user, string FirebasePath, SubToDo subtodo)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var subtodofirebase = await client.Child("subtodo").PostAsync(subtodo);
                    subtodo.Id = subtodofirebase.Key;
                    subtodo.User = user;

                    await client.Child("subtodo").Child(subtodofirebase.Key).PutAsync(subtodo);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = subtodo.Id,
                        Type = DocumentType.SubToDo,
                        User = user
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(subtodo);
            return subtodo.Id;
        }
        public async Task DeleteSubToDo(string user, string FirebasePath, SubToDo subtodo)
        {
            if (!user.Equals("guest"))
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
                        Type = DocumentType.SubToDo,
                        User = user
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(subtodo);
        }
        public async Task<string> CreateRoutineRegistry(string user, string FirebasePath, RoutineRegistry registry)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var registryfirebase = await client.Child("routineregistry").PostAsync(registry);
                    registry.Id = registryfirebase.Key;
                    registry.User = user;

                    await client.Child("routineregistry").Child(registryfirebase.Key).PutAsync(registry);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = registry.Id,
                        Type = DocumentType.RoutineRegistry,
                        User = user,
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(registry);
            return registry.Id;
        }
        public async Task DeleteRoutineRegistry(string user, string FirebasePath, RoutineRegistry registry)
        {
            if (!user.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.RoutineRegistry && x.Id.Equals(registry.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("routineregistry").Child(registry.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = registry.Id,
                        Type = DocumentType.RoutineRegistry,
                        User = user,
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(registry);
        }
    }
}
