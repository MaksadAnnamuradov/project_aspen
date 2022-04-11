﻿using AspenMobile.ViewModels;
using AspenMobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AspenMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }

       //private async void OnMenuItemClicked(object sender, EventArgs e)
       //{
       //    await Shell.Current.GoToAsync("//LoginPage");
       //}
    }
}
