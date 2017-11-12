using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Security.Cryptography; // lisatyt kirjastot alla
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;





// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {




        // NOTE: AES workkii aes.Key generoidulla avaimella -> pitää saada toimii käyttäjän syöttämällä avaimella 





        /*
        OHJELMAN TOIMINTA(talla hetkella): 
        Kayttaja valitsee salattavan tiedoston Open file-napilla 
        -> valitaan haluttu algoritmi radio-painikkeista 
        -> Encrypt-nappi toteuttaa salauksen
        -> 

        TODO:
        Encrypt painikkeen toiminta
        -> 1. vanhan tiedoston poisto jos sen Checkbox-checked
        -> 2. pitaa valita tiedostolle nimi ja kansio kun painetaan Encrypt-nappia,
        -> 3. jos vanhan tiedoston poisto Checkbox-unchecked ja kayttaja valitsee saman kansion jossa jo tiedosto samalla nimella -> -> toistetaan tiedoston tallennus ikkuna nimelle ja kansiolle
        
              
        

        PUUTTUU:
        salaukset, keyn tallennus muuttujaan, salatun stringin kasittely -> salattavan tiedoston tallennuskansion ja nimen valinta,
        vanhan tiedoston poisto(oma checkboxi sille)

        TOIMII:
        tiedoston(teksti) selaus/avaus, algoritmin valintanäppäimet, ohjelman sulkeminen
               
        */
        public MainPage()
        {
            this.InitializeComponent();
        }

        string fileContent;             // avatun tiedoston sisältö
        string selectedEncryption;      // valittu algoritmi
        string eText;                   // cryptattu teksti

        //tiedoston selaus/avaus/luku
        private async void Button_Click_Openfile(object sender, RoutedEventArgs e)
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            //openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add("*");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                textbox1.Text = "Opened file: " + file.Name;


                //lukee tiedoston sisallon muuttujaan ja tulostaa tekstin debugboxiin
                fileContent = await FileIO.ReadTextAsync(file);
                textbox4.Text = fileContent;

            }
            else
            {
                textbox1.Text = "Cancelled.";
            }

        }



        //encrypt nappia painettaessa suoritetaan valittu algoritmisalaus ja tiedoston tallennus

        public void Button_Click_Encrypt_file(object sender, RoutedEventArgs e)
        {
            string key = textbox3.Text;

            if (selectedEncryption == "Encryption1")
            {
                textbox4.Text = "alg 1 valittu";

                AESencrypt test = new AESencrypt();
                using (Aes aes = Aes.Create())
                {
                    string text = fileContent;
                    byte[] encrypted = test.Encrypt(text, aes.Key, aes.IV);
                    eText = String.Empty;
                    foreach (var b in encrypted)
                    {
                        eText += b.ToString() + " ";

                    }
                    textbox4.Text = eText;

                    SaveFile();
                }



            }

            //TripleDESencryptionin valinta, Tämä ei toimi tällä hetkellä koska compileri ei tunnista classia.

            else if (selectedEncryption == "Encryption2")
            {
                textbox4.Text = "alg 2 valittu";

                TripleDESencryption test = new TripleDESencryption();
                using (TripleDES tripledes = TripleDES.Create())
                {
                    string text = fileContent;
                    byte[] encrypted = test.Encrypt(text, tripledes.Key, tripledes.IV);
                    eText = String.Empty;
                    foreach (var b in encrypted)
                    {
                        eText += b.ToString() + " ";

                    }
                    textbox4.Text = eText;

                    SaveFile();
                }
            }
            else if (selectedEncryption == "Encryption3")
            {
                textbox4.Text = "alg 3 valittu";
            }

        }



        //ohjelman lopetus
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        //
        private void Alg1_selector_Checked(object sender, RoutedEventArgs e)
        {
            selectedEncryption = "Encryption1";
            textbox4.Text = "Selected Encryption1";
        }
        private void Alg2_selector_Checked(object sender, RoutedEventArgs e)
        {
            selectedEncryption = "Encryption2";
            textbox4.Text = "Selected Encryption2";
        }
        private void Alg3_selector_Checked(object sender, RoutedEventArgs e)
        {
            selectedEncryption = "Encryption3";
            textbox4.Text = "Selected Encryption3";
        }
        private void TextBox_TextChanged2(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBox_TextChanged3(object sender, TextChangedEventArgs e)
        {

        }






        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        public async void SaveFile()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
            Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);

                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, eText);

                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.







                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                /*
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile =
                    await storageFolder.CreateFileAsync(file.Name,
                        Windows.Storage.CreationCollisionOption.ReplaceExisting);

                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, eText);
                */


                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    this.textbox4.Text = "File " + file.Name + " was saved.";
                }
                else
                {
                    this.textbox4.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                this.textbox4.Text = "Operation cancelled.";
            }
        }


    }
}
