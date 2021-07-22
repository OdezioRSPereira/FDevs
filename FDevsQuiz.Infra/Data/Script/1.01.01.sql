IF OBJECT_ID('App_Contato') IS NULL BEGIN
Create Table App_Contato (
  Codigo             Numeric(10)  Identity(1, 1),
  Nome               Varchar(100) Null,
  SobreNome          Varchar(255) Null,
  Email              Varchar(100) Null,
  Telefone           Varchar(100) Null,
  DataNascimento     DateTime     Null,
  ImagemUrl          Varchar(255) Null,
  DataCadastro       DateTime     Null,
  DataAtualizacao    DateTime     Null
  Constraint App_Contato_PK Primary Key Clustered (Codigo)
)
END
Go

IF OBJECT_ID('App_Usuario') IS NULL BEGIN
Create Table App_Usuario (
  Codigo             Numeric(10)  Not Null,
  Senha              Varchar(255) Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint App_Usurio_PK Primary Key Clustered (Codigo),
  Constraint App_Usuario_Contato_FK Foreign Key (Codigo)
  References App_Contato (Codigo)
)
END
Go

IF OBJECT_ID('Enq_Nivel') IS NULL BEGIN
Create Table Enq_Nivel (
  Codigo             Numeric(10)  Not Null,
  Descricao          Varchar(100) Null,
  Constraint Enq_Nivel_PK Primary Key Clustered (Codigo)
)
END
Go

IF OBJECT_ID('Enq_Quiz') IS NULL BEGIN
Create Table Enq_Quiz (
  Codigo             Numeric(10)  Identity(1, 1),
  CodigoNivel        Numeric(10)  Not Null,
  Titulo             Varchar(255) Null,
  ImagemUrl          Varchar(255) Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint Enq_Quiz_PK Primary Key Clustered (Codigo),
  Constraint Enq_Quiz_Nivel_FK Foreign Key (CodigoNivel)
  References Enq_Nivel (Codigo)
)
END
Go

IF OBJECT_ID('Enq_Pergunta') IS NULL BEGIN
Create Table Enq_Pergunta (
  Codigo             Numeric(10)  Identity(1, 1),
  CodigoQuiz         Numeric(10)  Not Null,
  Titulo             Varchar(255) Null,
  OrdemExibicao      Numeric(3)   Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint Enq_Pergunta_PK Primary Key Clustered (Codigo),
  Constraint Enq_Pergunta_Quiz_FK Foreign Key (CodigoQuiz)
  References Enq_Quiz (Codigo)
)
END
Go

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('Enq_Pergunta') AND name='Enq_Pergunta_Quiz_FK') BEGIN
Create Index Enq_Pergunta_Quiz_FK On Enq_Pergunta(CodigoQuiz);
END
Go

IF OBJECT_ID('Enq_Alternativa') IS NULL BEGIN
Create Table Enq_Alternativa (
  Codigo             Numeric(10)  Identity(1, 1),
  CodigoPergunta     Numeric(10)  Not Null,
  Titulo             Varchar(255) Null,
  OrdemExibicao      Numeric(3)   Null,
  Correta            Bit          Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint Enq_Alternativa_PK Primary Key Clustered (Codigo),
  Constraint Enq_Alternativa_Pergunta_FK Foreign Key (CodigoPergunta)
  References Enq_Pergunta (Codigo)
)
END
Go

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('Enq_Alternativa') AND name='Enq_Alternativa_Pergunta_FK') BEGIN
Create Index Enq_Alternativa_Pergunta_FK On Enq_Alternativa(CodigoPergunta);
END
Go

IF OBJECT_ID('Enq_Resposta') IS NULL BEGIN
Create Table Enq_Resposta (
  Codigo             Numeric(10)  Identity(1, 1),
  CodigoContato      Numeric(10)  Not Null,
  CodigoAlternativa  Numeric(10)  Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint Enq_Resposta_PK Primary Key Clustered (Codigo),
  Constraint Enq_Resposta_Contato_FK Foreign Key (CodigoContato)
  References App_Contato (Codigo),
  Constraint Enq_Resposta_Alternativa_FK Foreign Key (CodigoAlternativa)
  References Enq_Alternativa (Codigo)
)
END
Go

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('Enq_Resposta') AND name='Enq_Resposta_Contato_FK') BEGIN
Create Index Enq_Resposta_Contato_FK On Enq_Resposta(CodigoContato);
END
Go

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id=OBJECT_ID('Enq_Resposta') AND name='Enq_Resposta_Alternativa_FK') BEGIN
Create Index Enq_Resposta_Alternativa_FK On Enq_Resposta(CodigoAlternativa);
END
Go

Insert Into Enq_Nivel (Codigo, Descricao) 
Select 1 As Codigo, 'Fácil' As Descricao Where Not Exists (Select 1 From Enq_Nivel Where Codigo = 1) Union All
Select 2 As Codigo, 'Intermediário' As Descricao Where Not Exists (Select 1 From Enq_Nivel Where Codigo = 2) Union All
Select 3 As Codigo, 'Difícil' As Descricao Where Not Exists (Select 1 From Enq_Nivel Where Codigo = 3) Union All
Select 4 As Codigo, 'Especialista' As Descricao Where Not Exists (Select 1 From Enq_Nivel Where Codigo = 4);
Go