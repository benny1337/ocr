using interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;
using businesscardapp.Communicator;
using models;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace businesscardapp.ViewModel
{
    public class StartViewModel:ViewModelBase
    {
        private IUserDialogService _dialog;
        private bool IsInitialized = false;
        private Action InitializedDone = null;
        private MediaFile _photo;        

        public StartViewModel(IUserDialogService dialog, CardCommunicator comm)
        {
            _comm = comm;
            _dialog = dialog;
            Task.Factory.StartNew(async () => 
            {
                await CrossMedia.Current.Initialize();
                IsInitialized = true;
                if (InitializedDone != null)
                    InitializedDone();
            });
        }

        private Command _takePhotoCommand;
        private CardCommunicator _comm;
                 
        public Command TakePhotoCommand
        {
            get
            {
                return _takePhotoCommand ?? (_takePhotoCommand = new Command(async() => 
                {    
                    await TakePhoto();
                }));
            }
        }

        private async Task GetCardAsync()
        {
            _dialog.ShowLoading();
            
            var stream = _photo.GetStream();
            if(stream.Length == 0)
            {
                var x = new StreamReader(stream);
                await x.ReadToEndAsync();                
            }
            var inArray = new Byte[(int)stream.Length];
            stream.Position = 0;
            stream.Read(inArray, 0, (int)stream.Length);
            
            try {                                        
                var base64 = Convert.ToBase64String(inArray);

                if(string.IsNullOrEmpty(base64))
                {
                    _dialog.Toast("Could not read image, please try again");
                    stream.Dispose();
                    return;
                }

                var card = await _comm.GetCardAsync(base64);
                             
                foreach (var prop in Properties)
                {
                    var p = card.GetType().GetRuntimeProperties().Where(x => x.Name.Equals(prop.Name));
                    prop.Data = p.First()?.GetValue(card) as string;
                }
                _dialog.HideDialog();
            } catch(Exception e)
            {
                _dialog.Toast(e.Message);                    
            }
            finally
            {                
                stream.Dispose();
            }
        }
        
        private Command<BusinessCardProperty> _swap;
        public Command<BusinessCardProperty> Swap
        {
            get
            {
                return _swap ?? (_swap = new Command<BusinessCardProperty>((control) =>
                {
                    var activecontrol = Properties.FirstOrDefault(x => x.IsActive);
                    if(activecontrol == null)
                        control.IsActive = control.IsActive ? false : true;
                    else
                    {
                        var temp = activecontrol.Data;
                        activecontrol.Data = control.Data;
                        control.Data = temp;

                        activecontrol.IsActive = false;
                        control.IsActive = false;
                    }
                }));
            }
        }

        private async Task TakePhoto()
        {
            if(!IsInitialized)
            {

            }

            if (CrossMedia.Current.IsCameraAvailable)
            {
                _photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {                    
                    Name = Guid.NewGuid().ToString(),
                    DefaultCamera = CameraDevice.Rear,
                    Directory = "businesscard",
                    SaveToAlbum = false
                });
            }
            else
            {
                _photo = await CrossMedia.Current.PickPhotoAsync();
            }
            
            if (_photo != null)
            {                
                await GetCardAsync();
            }   
        }

        #region business card properties
        private List<BusinessCardProperty> _properties;
        public List<BusinessCardProperty> Properties
        {
            get
            {
                return _properties ?? (_properties = new List<BusinessCardProperty>()
                {
                    new BusinessCardProperty()
                    {
                        Name = "Name",
                        Label = "Namn"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "Title",
                        Label = "Titel"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "CompanyName",
                        Label = "Företag"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "Email",
                        Label = "Email"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "Phone",
                        Label = "Tele"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "AddreessStreet",
                        Label = "Gata"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "AddressCity",
                        Label = "Ort"
                    },
                    new BusinessCardProperty()
                    {
                        Name = "Website",
                        Label = "Web"
                    },
                });
            }
        }
        #endregion
    }
}
