# GerenciadorCondominios
GerenciadorCondominios (revisitando mvc agora com .net core)

Projeto de estudo para verificar as mudanças do .net core em relação ao .net standard (posteriormente daremos um "tapa" na arquitetura e na lógica)

## Refatorar

- Arquitetura (tem camadas de negocio e de acesso a dados, mas o projeto web acessa ambas e a de negocio não me parece ter real sentido, pensar em um projeto para facilitar a injeção de dependencias e remover o acesso web a camada de dados deixando apenas acesso a de negocio);
- Arquitetura (mover a logica dos controladores para a camada de negocio, quando aplicavel, e nesse momento refatorar o que for preciso, eliminando principalmente a repetição de código);
- Eliminar os "try/cath" que levantam a própria exceção (isso nem faz sentido);
