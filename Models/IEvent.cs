
using MudBlazor;

namespace AppNotes.Models
{
    public interface IEvent
    {
        public string Id { get; set; }
        public string User { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Text { get; set; }
        public bool Done { get; set; }
        public DateTime Modified { get; set; }
        public string Icon { get; set; }
    }
}
