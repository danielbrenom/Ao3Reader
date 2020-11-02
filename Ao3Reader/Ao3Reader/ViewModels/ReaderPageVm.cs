using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;
using Ao3Reader.Models;

namespace Ao3Reader.ViewModels
{
    public class ReaderPageVm : BaseViewModel
    {
        private readonly IWorksService Service;
        
        public ReaderPageVm(INavigator navigator, IAlert alert, IWorksService service) : base(navigator, alert)
        {
            Service = service;
        }
        
        public ObservableCollection<Paragraph> Paragraphs { get; set; } = new ObservableCollection<Paragraph>();
        public WorkChapter Chapter { get; set; } = new WorkChapter();

        public bool Loaded { get; set; } = true;

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            if (parameters is null)
                return;
            if (parameters.TryGetValue("chapter", out var chapter) && parameters.TryGetValue("work", out var work))
            {
                var chapterId = int.Parse(((ChapterListing) chapter).Id);
                var workId = ((WorkIndexing) work).WorkId;
                var result = await Service.GetWorkChapter(workId,chapterId);
                result.ChapterParagraphs.ForEach(paragraph => Paragraphs.Add(new Paragraph{Text = paragraph}));
                Chapter.ChapterTitle = result.ChapterTitle;
                Loaded = false;
            }
        }
    }
}