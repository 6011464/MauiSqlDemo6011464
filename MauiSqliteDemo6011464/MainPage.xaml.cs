using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace MauiSqliteDemo6011464
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editClientesId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            LoadClientes();
        }

        private async void LoadClientes()
        {
            ListView.ItemsSource = await _dbService.GetClientes();
        }

        private async void saveButton_Clicked(object sender, EventArgs e) // Corregido nombre del método
        {
            if (_editClientesId == 0)
            {
                await _dbService.Create(new Cliente
                {
                    NombreCliente = nombreEntryField.Text,
                    Email = emailEntryField.Text,
                    Movil = movilEntryField.Text
                });
            }
            else
            {
                var cliente = await _dbService.GetById(_editClientesId);
                cliente.NombreCliente = nombreEntryField.Text;
                cliente.Email = emailEntryField.Text;
                cliente.Movil = movilEntryField.Text;
                await _dbService.Update(cliente);
                _editClientesId = 0;
            }

            nombreEntryField.Text = string.Empty;
            emailEntryField.Text = string.Empty;
            movilEntryField.Text = string.Empty;

            LoadClientes();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e) // Corregido nombre del evento
        {
            if (e.Item is Cliente cliente)
            {
                var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

                switch (action)
                {
                    case "Edit":
                        _editClientesId = cliente.Id;
                        nombreEntryField.Text = cliente.NombreCliente;
                        emailEntryField.Text = cliente.Email;
                        movilEntryField.Text = cliente.Movil;
                        break;

                    case "Delete":
                        await _dbService.Delete(cliente);
                        LoadClientes();
                        break;
                }
            }
        }
    }
}
