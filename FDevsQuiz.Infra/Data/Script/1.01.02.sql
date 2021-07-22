If Not Exists (Select * From Sys.Columns Where Object_ID = Object_ID('Enq_Resposta') And Name = 'CodigoPergunta')
Begin
  Alter Table Enq_Resposta Add CodigoPergunta Numeric(10),
  Constraint Enq_Reposta_Pergunta_FK Foreign Key (CodigoPergunta) 
    References Enq_Pergunta(Codigo);
End
Go

Update res
   set res.CodigoPergunta = alt.CodigoPergunta
  From Enq_Resposta res
 Inner Join Enq_Alternativa alt on (alt.Codigo = res.CodigoAlternativa)
 Where res.CodigoPergunta Is Null;