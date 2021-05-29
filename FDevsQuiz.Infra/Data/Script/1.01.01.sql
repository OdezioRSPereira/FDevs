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
Go

Create Table App_Usuario (
  Codigo             Numeric(10)  Not Null,
  Senha              Varchar(255) Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint App_Usurio_PK Primary Key Clustered (Codigo),
  Constraint App_Usuario_Contato_FK Foreign Key (Codigo)
  References App_Contato (Codigo)
)

Create Table Enq_Nivel (
  Codigo             Numeric(10)  Not Null,
  Descricao          Varchar(100) Null,
  Constraint Enq_Nivel_PK Primary Key Clustered (Codigo)
)
Go

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
Go

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
Go

Create Index Enq_Pergunta_Quiz_FK On Enq_Pergunta(CodigoQuiz);
Go

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
Go

Create Index Enq_Alternativa_Pergunta_FK On Enq_Alternativa(CodigoPergunta);
Go

Create Table Enq_ContatoAlternativa (
  Codigo             Numeric(10)  Identity(1, 1),
  CodigoContato      Numeric(10)  Not Null,
  CodigoAlternativa  Numeric(10)  Not Null,
  DataAtualizacao    DateTime     Null,
  DataCadastro       DateTime     Null
  Constraint Enq_ContatoAlternativa_PK Primary Key Clustered (Codigo),
  Constraint Enq_CAlternativa_Contato_FK Foreign Key (CodigoContato)
  References App_Contato (Codigo),
  Constraint Enq_CAlternativa_Alternativa_FK Foreign Key (CodigoAlternativa)
  References Enq_Alternativa (Codigo)
)
Go

Create Index Enq_CAlternativa_Contato_FK On Enq_ContatoAlternativa(CodigoContato);
Go

Create Index Enq_CAlternativa_Alternativa_FK On Enq_ContatoAlternativa(CodigoAlternativa);
Go

Insert Into Enq_Nivel (Codigo, Descricao) Values (1, 'Fácil');
Insert Into Enq_Nivel (Codigo, Descricao) Values (2, 'Intermediário');
Insert Into Enq_Nivel (Codigo, Descricao) Values (3, 'Difícil');
Insert Into Enq_Nivel (Codigo, Descricao) Values (4, 'Especialista');