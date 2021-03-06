﻿using Dasa.Laboratorios.Dominio.Commands;
using Dasa.Laboratorios.Dominio.Commands.LaboratorioCommands.Inputs;
using Dasa.Laboratorios.Dominio.Entidades;
using Dasa.Laboratorios.Dominio.ObjetosDeValor;
using Dasa.Laboratorios.Dominio.Repositorios;
using Dasa.Laboratorios.Shared;
using Dasa.Laboratorios.Shared.Commands;
using FluentValidator;

namespace Dasa.Laboratorios.Dominio.Handlers
{
    public class LaboratorioHandler : Notifiable, ICommandHandler<CriarLaboratorioCommand>
    {
        private readonly ILaboratorioRepositorio _laboratorioRepositorio;
        public LaboratorioHandler(ILaboratorioRepositorio laboratorioRepositorio)
        {
            _laboratorioRepositorio = laboratorioRepositorio;
        }

        public ICommandResult Handle(CriarLaboratorioCommand command)
        {
            //criar endereço
            var endereco = new Endereco(command.Logradouro,
                                        command.Numero,
                                        command.Bairro,
                                        command.Cidade,
                                        command.Estado,
                                        command.Cep);

            if (string.IsNullOrEmpty(command.Complemento))
                endereco.AdicionarComplemento(command.Complemento);


            //verificar se endereço é valido
            if (endereco.Invalid)
                return new CommandResult(false, Mensagens.CorrigirCampos, endereco.Notifications);


            //criar entidade laboratorio
            var laboratorio = new Laboratorio(command.Nome, endereco, command.Status);

            //verificar se entidade é valida
            if (laboratorio.Invalid)
                return new CommandResult(false, Mensagens.CorrigirCampos, laboratorio.Notifications);

            //Inserir
            _laboratorioRepositorio.Adicionar(laboratorio);

            return new CommandResult(true, Mensagens.LaboratorioCadastrado, new { });
            
        }
    }
}
