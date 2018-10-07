﻿using System;
using System.Windows.Forms;
using System.Drawing;
using Transitions;
using AcroniControls;
using System.Collections.Generic;
using AcroniLibrary.CustomizingMethods.TextFonts;
using AcroniLibrary.FileInfo;
using AcroniLibrary.DesignMethods;

namespace AcroniUI.Custom
{
    public partial class Compacto : Template
    {

        #region Declarações 


        // Definição do botão de teclado genérico (kbtn)
        Kbtn keybutton = new Kbtn();

        // Definição das propriedades de salvamento
        private bool SettedKeyboardProperties = false;
        Keyboard keyboard = new Keyboard();

        //Definição das propriedades das fontes
        ContentAlignment ContentAlignment { get; set; }
        private static List<FontFamily> lista_fontFamily = new List<FontFamily>();

        // Esse membro serve para pegar o ícone selecionado e botá-lo na fila de ícones.
        private Image SelectedIcon { get; set; }
        private bool HasChosenAIcon { get; set; }

        // Definição das propriedades do colorpicker 
        private Color Color { get; set; } = Color.FromArgb(26, 26, 26);


        #endregion

        #region Eventos a nível do formulário
        //Ao clicar num botão do teclado
        private void kbtn_Click(object sender, EventArgs e)
        {
            keybutton = (Kbtn)sender;
            keybutton.BackColor = keybutton.SetColor(Color);
            if (HasChosenAIcon)
                keybutton.Image = SelectedIcon;
        }

        //Ao clicar no botão de fechar
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            SelectKeyboard __selectKeyboard = new SelectKeyboard();
            __selectKeyboard.ShowDialog();
            //Compartilha.editKeyboard = false;
            this.Close();
        }

        private void btnVoltar_MouseMove(object sender, MouseEventArgs e)
        {
            btnVoltar.Font = new Font(btnVoltar.Font, FontStyle.Underline | FontStyle.Bold);
        }

        private void btnVoltar_MouseLeave(object sender, EventArgs e)
        {
            btnVoltar.Font = new Font(btnVoltar.Font, FontStyle.Bold);
        }

        #endregion

        public Compacto()
        {
            InitializeComponent();

            //Foreach para arredondar cores do colorpicker
            foreach (Control c in pnlCorEscolhida.Controls)
            {
                if (c is Button)
                {
                    Bunifu.Framework.UI.BunifuElipse elipse = new Bunifu.Framework.UI.BunifuElipse();
                    elipse.ApplyElipse(c, 5);
                }
            }

            //if (Compartilha.editKeyboard)
            //    LoadKeyboard();
            //else
            //    ApplyShadowOnKeycaps();

        }

        private void ApplyShadowOnKeycaps()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Kbtn)
                {
                    try
                    {
                        Controls.Find("fundo" + c.Name, true)[0].BackColor = Color.FromArgb(90, c.BackColor);
                    }
                    catch (Exception) { }
                }
            }
        }

        // Ideia pro módulo de texto das keycaps
        //private void SetKeycapText(object sender, EventArgs e)
        //{
        //KeycapTextModule keycapTextModule = new KeycapTextModule();
        //keycapTextModule.ShowDialog();
        //Kbtn kbtn = (Kbtn)sender;
        //kbtn.Text = keycapTextModule.Maintext;
        //}


        #region Métodos do Color Picker

        private bool[] __disponibilidade_pnlHistorico { get; set; } = { true, false, false };

        //Clicks que ocorrem ao selecionar uma cor
        private void Color_Click(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            lblHexaColor.Text = $"#{p.BackColor.R.ToString("X2")}{p.BackColor.G.ToString("X2")}{p.BackColor.B.ToString("X2")}";
            lblColorName.Text = (!p.Name.Contains("pnl") ? p.Name.Replace("_", " ") : p.Tag.ToString());

            //--Transição para mudar de cor
            Transition transition = new Transition(new TransitionType_EaseInEaseOut(200));
            transition.add(pnlColor, "BackColor", p.BackColor);
            transition.run();

            //Define a cor da tecla no kbtn_Click
            Color = p.BackColor;

            if (__disponibilidade_pnlHistorico[0])
            {
                pnlHistorico1.BackColor = p.BackColor;
                __disponibilidade_pnlHistorico[0] = false;
                __disponibilidade_pnlHistorico[1] = true;
            }
            else if (__disponibilidade_pnlHistorico[1])
            {
                pnlHistorico2.BackColor = p.BackColor;
                __disponibilidade_pnlHistorico[1] = false;
                __disponibilidade_pnlHistorico[2] = true;
            }
            else if (__disponibilidade_pnlHistorico[2])
            {
                pnlHistorico3.BackColor = p.BackColor;
                __disponibilidade_pnlHistorico[2] = false;
                __disponibilidade_pnlHistorico[0] = true;
            }
        }

        #region Hover para cada uma das cores do colorpicker


        private void pnlColor_MouseMove(object sender, MouseEventArgs e)
        {
            //if (checkIfItsFirstTime == 0)
            //    previousColor = pnlColor.BackColor;
            //checkIfItsFirstTime++;
            //pnlColor.BackColor = Color.FromArgb(20, pnlColor.BackColor);
            //foreach (Control c in this.Controls)
            //{
            //    if (!c.Name.Contains("pnlCo") || !c.Name.Equals("pnlHeadColorpicker"))
            //        c.Visible = false;
            //}
        }

        #endregion


        #endregion

        #region Fontes das teclas e texto




        #region Definição dos métodos de alinhamento

        private void btnTextAlignLeft_Click(object sender, EventArgs e)
        {
            ContentAlignment = ContentAlignment.TopLeft;
        }

        private void btnTextAlignCenter_Click(object sender, EventArgs e)
        {
            if (ContentAlignment == ContentAlignment.TopCenter)
                ContentAlignment = ContentAlignment.TopLeft;
            else
                ContentAlignment = ContentAlignment.TopCenter;
        }

        private void btnTextAlignRight_Click(object sender, EventArgs e)
        {
            if (ContentAlignment == ContentAlignment.TopRight)
                ContentAlignment = ContentAlignment.TopLeft;
            else
                ContentAlignment = ContentAlignment.TopRight;
        }
        #endregion

        //#region ComboBox de FontFace
        //protected virtual void cmbFontes_SelectedIndexChanged(object sender, EventArgs e) => FontSender = new Font(cmbFontes.Text, FontSize, FontStyle);

        private void FormLoad(object sender, EventArgs e)
        {
            Fade.FadeIn(this);

            //Carregar todas as fontes que o usuário possui na máquina
            new LoadFontTypes(ref cmbFontes, ref lista_fontFamily);

            //Index padrão da combobox
            cmbFontes.SelectedIndex = cmbFontes.Items.IndexOf("Open Sans");
        }

        private void lblDefinirParaTodasTeclas_Click(object sender, EventArgs e)
        {

        }

        #endregion Fim das fontes

        //#region Carrega Teclado
        //private void LoadKeyboard()
        //{
        //    picBoxKeyboardBackground.Image = CompartilhaObjetosUser.teclado.BackgroundImage;
        //    picBoxKeyboardBackground.SizeMode = (PictureBoxSizeMode)CompartilhaObjetosUser.teclado.BackgroundModeSize;
        //    foreach (Control tecla in this.Controls)
        //    {
        //        if (tecla is Kbtn)
        //        {
        //            foreach (Keycaps keycap in CompartilhaObjetosUser.teclado.Keycaps)
        //            {
        //                if (tecla.Name.Equals(keycap.ID))
        //                {
        //                    tecla.Font = keycap.Font;
        //                    tecla.BackColor = keycap.Color;
        //                    tecla.Text = keycap.Text;
        //                    (tecla as Button).TextAlign = (ContentAlignment)keycap.ContentAlignment;
        //                    try
        //                    {
        //                        Controls.Find("fundo" + tecla.Name, true)[0].BackColor = Color.FromArgb(90, tecla.BackColor);
        //                    }
        //                    catch (Exception) { }
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion

        //#region Salvamento e Carregamento teclados

        //private void btnLer_Click(object sender, EventArgs e)
        //{
        //    using (FileStream openarchive = new FileStream(Application.StartupPath + @"\" + Conexao.nome_usuario +".acr", FileMode.Open))
        //    {
        //        BinaryFormatter ofByteArrayToObject = new BinaryFormatter();
        //        colecao = (AcroniLibrary.FileInfo.Colecao)ofByteArrayToObject.Deserialize(openarchive);

        //    }

        //    pictureBox1.Image = colecao.collection[0].BackgroundImage;
        //    pictureBox1.SizeMode = (PictureBoxSizeMode)colecao.collection[0].BackgroundModeSize;
        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is Kbtn)
        //        {
        //            foreach (Keycaps tecla in this.colecao.collection[0].Keycaps)
        //            {
        //                if (control.Name.Equals(tecla.ID))
        //                {
        //                    control.Name = tecla.ID;
        //                    control.Font = tecla.Font;
        //                    control.BackColor = tecla.Color;
        //                    control.Text = tecla.Text;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            //    if (!Compartilha.editKeyboard)
            //    {
            //        AcroniMessageBoxInput nameteclado = new AcroniMessageBoxInput("Insira o nome de seu teclado", "");
            //        nameteclado.Show();
            //        while (nameteclado.Visible)
            //        {
            //            await Task.Delay(100);
            //        }
        }
        //    SaveKeyboard();


        //}
        //private async void SaveKeyboard()
        //{
        //    if (!Compartilha.editKeyboard)
        //    {
        //        if (SetNames.teclado != null)
        //        {
        //            Galeria selectGaleria = new Galeria(true);
        //            selectGaleria.Show();
        //            while (selectGaleria.Visible)
        //            {
        //                await Task.Delay(100);
        //            }
        //            if (SetNames.colecao != null)
        //            {
        //                setPropriedadesTeclado();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (AcroniLibrary.FileInfo.Colecao c in CompartilhaObjetosUser.user.userCollections)
        //        {
        //            if (Compartilha.colecao.Equals(c.collectionNome))
        //            {
        //                c.collection.Remove(CompartilhaObjetosUser.teclado);

        //            }
        //        }
        //        setPropriedadesTeclado();


        //    }

        //    if (SetNames.colecao != null && SetNames.teclado != null || SettedKeyboardProperties)
        //    {
        //        System.Windows.MessageBox.Show("Teclado adicionado/salvo com sucesso!");
        //        Compartilha.editKeyboard = true;
        //        CompartilhaObjetosUser.teclado = keyboard;
        //    }
        //    else
        //        System.Windows.MessageBox.Show("Teclado não foi salvo! Você ser lix");
        //    SetNames.colecao = null;
        //    SetNames.teclado = null;
        //}
        //private void setPropriedadesTeclado()
        //{
        //    keyboard.Name = "FX-4370";
        //    if (Compartilha.editKeyboard)
        //        keyboard.NickName = CompartilhaObjetosUser.teclado.NickName;
        //    else
        //        keyboard.NickName = SetNames.teclado;
        //    keyboard.Material = "Madeira";
        //    keyboard.isMechanicalKeyboard = true;
        //    keyboard.hasRestPads = false;
        //    keyboard.BackgroundImage = picBoxKeyboardBackground.Image;
        //    keyboard.BackgroundModeSize = picBoxKeyboardBackground.SizeMode;
        //    keyboard.ID = "ID";
        //    keyboard.tipoTeclado = this.Name;
        //    foreach (Control tecla in this.Controls)
        //        if (tecla is Kbtn)
        //        {
        //            {
        //                keyboard.Keycaps.Add(new Keycaps { ID = tecla.Name, Text = tecla.Text, Font = tecla.Font, Color = tecla.BackColor, ContentAlignment = (tecla as Button).TextAlign });
        //            }
        //        }
        //    if (!Compartilha.editKeyboard)
        //        foreach (AcroniLibrary.FileInfo.Colecao c in CompartilhaObjetosUser.user.userCollections)
        //        {
        //            if (c.collectionNome.Equals(SetNames.colecao))
        //            {
        //                c.collection.Add(keyboard);
        //                break;
        //            }
        //        }
        //    else
        //        foreach (AcroniLibrary.FileInfo.Colecao c in CompartilhaObjetosUser.user.userCollections)
        //        {
        //            if (c.collectionNome.Equals(Compartilha.colecao))
        //            {
        //                c.collection.Add(keyboard);
        //                break;
        //            }
        //        }

        //    using (FileStream savearchive = new FileStream(Application.StartupPath + @"\" + SQLConnection.nome_usuario + ".acr", FileMode.OpenOrCreate))
        //    {
        //        BinaryFormatter Serializer = new BinaryFormatter();
        //        Serializer.Serialize(savearchive, CompartilhaObjetosUser.user);
        //    }
        //    SettedKeyboardProperties = true;
        //}
        //#endregion

        #region Ícones

        private Queue<Image> ImageQueue = new Queue<Image>();

        private void btnIcons_Click(object sender, EventArgs e)
        {
            List<Image> insertableArray = new List<Image> { };
            using (OpenFileDialog iconGetter = new OpenFileDialog())
            {
                iconGetter.InitialDirectory = @"C:\";
                iconGetter.Title = "Qual o ícone que deseja adicionar?";
                iconGetter.Filter = "Todos os tipos de imagem | *jpg; *.jpeg; *.bmp; *.png |BMP | *.bmp | JPG | *.jpg; *.jpeg | PNG | *.png ";
                iconGetter.Multiselect = true;
                if (iconGetter.ShowDialog() == DialogResult.OK)
                {
                    foreach (String fileDirectory in iconGetter.FileNames)
                        ImageQueue.Enqueue(Image.FromFile(fileDirectory));

                    while (ImageQueue.Count > 10)
                        ImageQueue.Dequeue();
                }
                for (int aux = ImageQueue.Count - 1; aux >= 0; aux--)
                {
                    insertableArray.Add(ImageQueue.ToArray()[aux]);
                }
            }
            //for (int i = 0; i < ImageQueue.Count; i++)
            //(pnlIcons.Controls[$"picBoxIcon{i + 1}"] as PictureBox).Image = insertableArray[i];
        }

        private void picIcons_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                PictureBox __icon = (PictureBox)sender;
                SelectedIcon = __icon.Image;
                HasChosenAIcon = true;
            }
        }

        #endregion

        private void ChangeColorFundoKbtn(object sender, PaintEventArgs e)
        {
            try
            {
                Controls.Find("fundo" + keybutton.Name, true)[0].BackColor = Color.FromArgb(90, keybutton.BackColor);
            }
            catch (Exception) { }
        }

        private void btnColor_MouseLeave(object sender, EventArgs e)
        {
            Button btnColor = (Button)sender;
            btnColor.Size = new Size(btnColor.Size.Width - 5, btnColor.Size.Height - 5);
            btnColor.Location = new Point(btnColor.Location.X + (5 / 2), btnColor.Location.Y + (5 / 2));
            Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();
            bunifuElipse.ApplyElipse(btnColor, 5);
        }

        private void btnColor_MouseEnter(object sender, EventArgs e)
        {
            Button btnColor = (Button)sender;
            btnColor.Size = new Size(btnColor.Size.Width + 5, btnColor.Size.Height + 5);
            btnColor.Location = new Point(btnColor.Location.X - (5 / 2), btnColor.Location.Y - (5 / 2));
            Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();
            bunifuElipse.ApplyElipse(btnColor, 5);
        }
    }
}

