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
        public async Task<string> CreateEvent(string user, string FirebasePath, Event evento)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var eventfirebase = await client.Child("event").PostAsync(evento);
                    evento.Id = eventfirebase.Key;
                    evento.User = user;

                    await client.Child("event").Child(eventfirebase.Key).PutAsync(evento);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = evento.Id,
                        Type = DocumentType.Event,
                        User = user,
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(evento);
            return evento.Id;
        }
        public async Task DeleteEvent(string user, string FirebasePath, Event evento)
        {
            if (!user.Equals("guest"))
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
                        Type = DocumentType.Event,
                        User = user
                    };
                    _conn.Conn.Insert(item);
                }
            }
            _conn.Conn.Delete(evento);
        }

    }
}
