
using AutoMapper;
using Azure;
using TrainingX_2.Models.Base;
using TrainingX_2.Models.Training;
using TrainingX_2.ViewModels.CategoriesViewModel;
using TrainingX_2.ViewModels.TrainingsViewModel;

namespace TrainingX_2.Helpers
{
    public class Helper:Profile
    {
        public Helper() 
        {
            CreateMap<Category, CategoriesViewModel>(); //Show
            CreateMap<CategoriesViewModel, Category>(); //Create
            CreateMap<Category, CategoriesViewModel>().ReverseMap(); //Edit delete

            CreateMap<Training, TrainingsViewModel>(); //Show
            CreateMap<TrainingsViewModel, Training>(); //Create
            CreateMap<Training, TrainingsViewModel>().ReverseMap(); //Edit delete
        }
    }
}
