# MedVet - 2TDSPJ

## Integrantes do Grupo

| Nome                   | RM       |
|------------------------|----------|
| João Henrique Batista  | RM564361 |
| Gutemberg Rocha        | RM562267 |
| Erik Miyasato          | RM565771 |


### Domínio Escolhido

Clínica de Medicina Veterinária

### SGDB da equipa
Oracle


### Entidades Modeladas

Foram modeladas 7 (sete) entidades principais:

| Entidade | Descrição | PK |
|----------|-----------|----|
| **Dono** | Proprietário dos animais | Id, Nome, Email, Telefone |
| **Pet** | Animais de estimação | Id, Nome, TipoAnimal, Raça, Genero, |
| **Veterinario** | Médicos veterinários | Id, Nome, CRM, Especialidade |
| **Medicamento** | Remédios disponíveis | Id, Nome, Marca, ModoDeUso, Preço |
| **Consulta** | Atendimentos realizados | Id, DataConsulta, Diagnóstico, Observações |
| **Prescricao** | Receitas médicas | Id |


### Resumo dos Relacionamentos

- Dono pode ter vários Pets (1:N)

- Pet pertence a um Dono (N:1) - OBRIGATÓRIO

- Pet pode ter várias Consultas (1:N)

- Veterinario pode realizar várias Consultas (1:N)

- Consulta é de um Pet (N:1) - OBRIGATÓRIO

- Consulta tem um Veterinario (N:1) - OBRIGATÓRIO

- Consulta tem uma Prescrições (1:1)

- Prescrição pertence a uma Consulta (1:1) - OBRIGATÓRIO

- Prescrição contém um tipo de Medicamento (N:1) - OBRIGATÓRIO

- Um tipo de Medicamento pode estar em várias Prescrições (1:N)


