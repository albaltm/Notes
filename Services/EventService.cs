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
    public class EventService(ConexionBBDD _conn)
    {
        public async Task<string> CreateEvent(string token, string FirebasePath, Event evento)
        {
            if (!token.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var userId = (await client.Child("userauthentication").OnceAsync<UserAuthentication>()).Select(x => x.Object).Where(x => x.Token.Equals(token)).ToList().FirstOrDefault()?.User;
                    var eventfirebase = await client.Child("event").PostAsync(evento);
                    evento.Id = eventfirebase.Key;
                    evento.User = userId;

                    await client.Child("event").Child(eventfirebase.Key).PutAsync(evento);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = evento.Id,
                        Type = DocumentType.Event
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(evento);
            return evento.Id;
        }
        public async Task DeleteEvent(string token, string FirebasePath, Event evento)
        {
            if (!token.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Event && x.Id.Equals(evento.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("event").Child(evento.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = evento.Id,
                        Type = DocumentType.Event
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(evento);
        }

    }
}
