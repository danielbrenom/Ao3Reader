using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Ao3Domain.Models.Data;
using Ao3Reader.Interfaces;
using Ao3Reader.Models;
using Xamarin.Forms;

namespace Ao3Reader.ViewModels
{
    public class ReaderPageVm : BaseViewModel
    {
        private readonly IWorksService Service;
        public ICommand ReturnCommand { get; set; }
        public ICommand NightCommand { get; set; }
        
        public ReaderPageVm(INavigator navigator, IAlert alert, IWorksService service) : base(navigator, alert)
        {
            Service = service;
            ReturnCommand = new Command(async() => await Navigator.ReturnAsync());
            NightCommand = new Command(async () => await NightShift());
        }
        
        public ObservableCollection<Paragraph> Paragraphs { get; set; } = new ObservableCollection<Paragraph>();
        public WorkChapter Chapter { get; set; } = new WorkChapter();

        public bool Loaded { get; set; } = true;
        public bool NightMode { get; set; }

        public override async Task HandleNavigation(IReadOnlyDictionary<string, object> parameters = null)
        {
            if (parameters is null)
                return;
            if (parameters.TryGetValue("chapter", out var chapter) && parameters.TryGetValue("work", out var work))
            {
                var chapterId = int.Parse(((ChapterListing) chapter).Id);
                var workId = ((WorkIndexing) work).WorkId;
                var result = await Service.GetWorkChapter(workId,chapterId);
                result.ChapterParagraphs.ForEach(paragraph => Paragraphs.Add(new Paragraph {Text = paragraph}));
                Chapter.ChapterTitle = result.ChapterTitle;
                Loaded = false;
            }

            NightMode = Application.Current.UserAppTheme == OSAppTheme.Dark;
        }

        private async Task NightShift()
        {
            await Task.Run(() =>
            {
                NightMode = !NightMode;
                Application.Current.UserAppTheme = NightMode ? OSAppTheme.Dark : OSAppTheme.Light;
            });
        }
    }
}