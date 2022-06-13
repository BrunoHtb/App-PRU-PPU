using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Xamarin.Forms;


namespace rodovias.DAL
{
    public class ClsPopular
    {
        public void ComboxDR(Picker DR)
        {
            DAL.Conectar();
            string cmd = string.Format("SELECT DISTINCT dr FROM public.dr order by dr asc ;");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //DR.ItemDisplayBinding = "dr";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DR.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em DR" + ex.Message, "OK");
            }
            finally
            {
                DAL.Desconectar();
            }
        }

        public void ComboxRodoviaPrefixo(Picker Rodovia)
        {
            if (Rodovia.Items.Count > 0)
            {
                Rodovia.Items.Clear();
            } 
            DAL.Conectar();
            string cmd = string.Format("SELECT DISTINCT prefixo FROM public.dr order by prefixo asc;");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Rodovia.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em Prefixo Rodovia" + ex.Message, "OK");
            }
            finally
            {
                DAL.Desconectar();
            }
        }

        public void ComboxRodovia(Picker Rodovia)
        {
            if (Rodovia.Items.Count > 0)
            {
                Rodovia.Items.Clear();
            }
            DAL.Conectar();
            string cmd = string.Format($"SELECT DISTINCT rodovia FROM public.dr;");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Rodovia.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em Rodovia" + ex.Message, "OK");
            }
            finally
            {
                DAL.Desconectar();
            }
        }

        public void ComboxRodoviaRodovia(Picker Rodovia, string _prefixo, string _dr)
        {
            if (Rodovia.Items.Count > 0)
            {
                Rodovia.Items.Clear();
            }
            DAL.Conectar();
            string cmd = string.Format($"SELECT DISTINCT rodovia FROM public.dr where prefixo LIKE '%{_prefixo}';");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    //DR.ItemDisplayBinding = "dr";
                   
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Rodovia.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em Rodovia" + ex.Message, "OK");
            }
            finally
            {
                DAL.Desconectar();
            }
        }

        public void ComboxEditRegional(Picker Regional)
        {
            if (Regional.Items.Count > 0)
            {
                Regional.Items.Clear();
            }
            DAL.Conectar();
            string cmd = string.Format($"SELECT DISTINCT regional FROM public.dr;");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Regional.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em Regional" + ex.Message, "OK");
                return;
            }
            finally
            {
                DAL.Desconectar();
            }
        }

        public void ComboxRegional(Picker Regional, string _NomeRodovia)
        {
            if (Regional.Items.Count > 0)
            {
                Regional.Items.Clear();
            }
            DAL.Conectar();
            string cmd = string.Format($"SELECT DISTINCT regional FROM public.dr where rodovia = '{_NomeRodovia}';");
            try
            {
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, DAL.Conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Regional.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Desculpe. Erro ao preencher opções em Regional" + ex.Message, "OK");
                return;
            }
            finally
            {
                DAL.Desconectar();
            }
        }
    }
}
