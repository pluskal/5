﻿using System;
using System.Collections.Generic;
using System.Linq;
using CookBook.BL;
using CookBook.BL.Messages;
using CookBook.BL.Models;
using CookBook.BL.Repositories;
using CookBook.DAL.Entities;

namespace CookBook.App.ViewModels
{
    public class RecipeDetailViewModel : ViewModelBase
    {
        private readonly RecipeRepository _recipeRepository;
        private readonly IMessenger _messenger;
        private RecipeDetailModel _detail;

        public RecipeDetailModel Detail
        {
            get { return _detail; }
            set
            {
                if (Equals(value, Detail)) return;

                _detail = value;
                OnPropertyChanged();
            }
        }

        public IList<FoodType> FoodTypes => Enum.GetValues(typeof(FoodType)).Cast<FoodType>().ToList();

        public RecipeDetailViewModel(RecipeRepository recipeRepository, IMessenger messenger)
        {
            _recipeRepository = recipeRepository;
            _messenger = messenger;

            _messenger.Register<SelectedRecipeMessage>(SelectedRecipe);
            _messenger.Register<NewRecipeMessage>(NewRecipeMessageReceived);

            // ToDo: Add commands to save and delete

        }

        private void NewRecipeMessageReceived(NewRecipeMessage message)
        {
            Detail = new RecipeDetailModel();
        }

        private void SelectedRecipe(SelectedRecipeMessage message)
        {
            Detail = _recipeRepository.GetById(message.Id);
        }

        private bool IsSavedRecipe()
        {
            return Detail.Id != Guid.Empty;
        }
    }
}