﻿using Caliburn.Micro;
using ComicReaderClassLibrary.DataAccess.DataModels;
using ComicReaderDataManagementUI.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ComicReaderDataManagementUI.ViewModels
{
    public interface ILanguageViewModel
    {
        /// <summary>
        /// Backing field for the Statusbar label
        /// </summary>
        public string StatusBar { get; }
        /// <summary>
        /// Backing field for the Comic dropdown
        /// </summary>
        BindableCollection<ComicDataModel> ComicBox { get; }
        /// <summary>
        /// Backing field for the Comics in Language listbox
        /// </summary>
        BindableCollection<ComicDataModel> ComicsInLanguageBox { get; }
        /// <summary>
        /// Backing field for the Language dropdown
        /// </summary>
        BindableCollection<LanguageDataModel> LanguageBox { get; }
        /// <summary>
        /// Backing field for the Language term textbox
        /// </summary>
        string LanguageName { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic dropdown
        /// </summary>
        ComicDataModel SelectedComic { get; set; }
        /// <summary>
        /// Backing field for the Selected Comic in the Comic in Language Listbox
        /// </summary>
        ComicDataModel SelectedComicInLanguage { get; set; }
        /// <summary>
        /// Backing field for the Selected Language in the Language dropdown
        /// </summary>
        LanguageDataModel SelectedItem { get; set; }

        /// <summary>
        /// Method for the Add Comic to Language button
        /// </summary>
        void AddComicToLanguage();
        /// <summary>
        /// Method for the New Language button
        /// </summary>
        void NewLanguage();
        /// <summary>
        /// Method for the Save Language button
        /// </summary>
        void SaveLanguage();
        /// <summary>
        /// Method for the Delete Language button
        /// </summary>
        void DeleteLanguage();
        /// <summary>
        /// Event handler for when a Comic was changed in the MainViewModel
        /// </summary>
        Task HandleAsync(ComicChangedOnMainEvent message, CancellationToken cancellationToken);
    }
}