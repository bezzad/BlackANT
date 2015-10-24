using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace PictureBoxStatus
{
    public partial class PicBoxStatus: UserControl
    {
        #region Members
        private System.Windows.Forms.Timer timer = new Timer();
        private float degree = 0;
        private float stepper = 1f;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance"), Description("NotLoad Image for NotLoad Status in Bitmap format.")]
        [DisplayName("Image NotLoad")]
        public Bitmap ImageNotLoad
        {
            get
            {
                return (imageNotLoad == null) ? 
                    global::PictureBoxStatus.Properties.Resources.SmallRed : 
                    imageNotLoad;
            }
            set
            {
                if (value != null)
                {
                    imageNotLoad = value;
                    if (CurrentStatus == ImageState.NotLoad) this.picBox.Image = value;
                }
            }
        }
        private Bitmap imageNotLoad;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance"), Description("Loaded Image for Loaded Status in Bitmap format.")]
        [DisplayName("Image Loaded")]
        public Bitmap ImageLoaded
        {
            get
            {
                return (imageLoaded == null) ?
                    global::PictureBoxStatus.Properties.Resources.SmallGreen :
                    imageLoaded;
            }
            set
            {
                if (value != null)
                {
                    imageLoaded = value;
                    if (CurrentStatus == ImageState.Loaded) this.picBox.Image = value;
                }
            }
        }
        private Bitmap imageLoaded;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance"), Description("Loading Image for Loading Status in Bitmap format.")]
        [DisplayName("Image Loading")]
        public Bitmap ImageLoading
        {
            get
            {
                return (imageLoading == null) ?
                    global::PictureBoxStatus.Properties.Resources.block_loading :
                    imageLoading;
            }
            set
            {
                if (value != null)
                {
                    imageLoading = value;

                    if (value.RawFormat.ToString() == "[ImageFormat: b96b3cb0-0728-11d3-9d7b-0000f81ef32e]") // is .Gif ?
                        LoadingImageAnimated = false;
                    else LoadingImageAnimated = true;

                    if (CurrentStatus == ImageState.Loading) this.picBox.Image = value;
                }
            }
        }
        private Bitmap imageLoading;
        

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Appearance"), Description("Set or Get Current Status of Load Mission.")]
        [DisplayName("Current Status"), AmbientValue(typeof(ImageState), "NotLoad")]
        public ImageState CurrentStatus {
            get { return currentStatus; }
            set
            {
                currentStatus = value;
                SetState(value);
            }
        }
        private ImageState currentStatus = ImageState.NotLoad;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Behavior"), Description("Loading State Animation Speed.\n\r Note: Effective speed range is between 1 ~ 359999")]
        [DisplayName("Loading Animate Speed"), AmbientValue(1000)]
        public int LoadingAnimateSpeed
        {
            get { return loadingAnimateSpeed; }
            set
            {
                loadingAnimateSpeed = value;
                //
                if (value < 1)
                {
                    stepper = 1f;
                    timer.Interval = 1000;
                }
                else if (value > 1000 && value < 359999)
                {
                    stepper = (float)value / 1000;
                    timer.Interval = 1;
                }
                else if (value > 359999)
                {
                    stepper = 359f;
                    timer.Interval = 1;
                }
                else // value < 1001 && value > 0
                {
                    stepper = 1f;
                    timer.Interval = 1001 - value;
                }
            }
        }
        private int loadingAnimateSpeed;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Behavior"), Description("Loading State Animation is ClockWise or No? (Default is true = ClockWise)")]
        [DisplayName("ClockWise Animate?"), AmbientValue(true), DefaultValue(true)]
        public bool ClockWise { get; set; }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool LoadingImageAnimated { get; set; }
        #endregion

        public PicBoxStatus()
        {
            InitializeComponent();            
            
            ClockWise = true;
            LoadingAnimateSpeed = 1000;

            timer.Tick += (source, e) =>
            {
                picBox.RotateImage(ImageLoading, checked(degree += (ClockWise) ? -stepper : stepper));
            };
        }


        #region Methods

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private void Start()
        {
            if (!this.timer.Enabled && LoadingImageAnimated)
                this.timer.Start();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private void Stop()
        {
            if (this.timer.Enabled)
                this.timer.Stop();
        }
        #endregion

        /// <summary>
        /// Set picBoxStatus Image by ImageState
        /// </summary>
        /// <param name="state">State of Image View</param>
        protected void SetState(ImageState state)
        {
            switch (state)
            {
                case ImageState.NotLoad:
                    {
                        this.picBox.Image = this.ImageNotLoad;
                        Stop();
                    }
                    break;
                case ImageState.Loading:
                    {
                        this.picBox.Image = this.ImageLoading;
                        Start();
                        
                    }
                    break;
                case ImageState.Loaded:
                    {
                        this.picBox.Image = this.ImageLoaded;
                        Stop();
                    }
                    break;
                default:
                    {
                        this.picBox.Image = this.ImageNotLoad;
                        Stop();
                    }
                    break;
            }
        }
    }
}
