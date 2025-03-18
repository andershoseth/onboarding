using onboarding_backend.Services;

namespace onboarding_backend.Models
{
    public class GroupedSafTEntries
    {
        public string GroupKey { get; set; }
        public List<FlattenedEntry> Entries { get; set; }
    }
}
