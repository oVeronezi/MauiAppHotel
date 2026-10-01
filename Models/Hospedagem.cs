using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppHotel.Models
{
    public class Hospedagem
    {
        Quarto _quarto_selecionado;
        public Quarto QuartoSelecionado
        {
            get => _quarto_selecionado;
            set
            {
                // se o valor for nulo, dispara a exception
                if (value == null)
                    throw new Exception("Favor de selecionar um quarto");

                _quarto_selecionado = value;
            }
        }

        int _qnt_adultos;
        public int QntAdultos 
        {
            get => _qnt_adultos;
            set
            {
                if (value <= 0) 
                    throw new Exception("Favor colocar quantos adultos no quarto");

                _qnt_adultos = value;
            }
        }
        public int QntCriancas { get; set; }
        DateTime? _data_checkin;
        public DateTime? DataCheckin 
        {
            get => _data_checkin;
            set
            {
                if (value == null)
                    throw new Exception("Por favor, selecione a data de entrada no hotel");

                _data_checkin = value;
            }
        }
        DateTime? _data_checkout;
        public DateTime? DataCheckout 
        {
            get => _data_checkout;
            set
            {
                if (value == null)
                    throw new Exception("Por favor, selecione a data de saída no hotel");

                _data_checkout = value;
            }
        }
        public int Estadia
        {
            // transforma as datas em valores e subtrai
            get => DataCheckout.Value.Subtract(DataCheckin.Value).Days;
        }
        public double ValorTotal
        {
            get
            {
                double valor_adultos = QntAdultos * QuartoSelecionado.ValorDiariaAdulto;
                double valor_criancas = QntCriancas * QuartoSelecionado.ValorDiariaCrianca;
                double total = (valor_adultos + valor_criancas) * Estadia;
                return total;
            }
        }
    }
}
