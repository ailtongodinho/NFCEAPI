# NFCE API
Projeto NFCEAPI para leitura de Notas Fiscais Eletrônicas.

Desenvolvido em DotnetCore utilizando recursos do Heroku, como Heroku Dynos e Heroku Add-ons (Postgres Database).

---
## Como funciona

A API faz um web scraping da Nota Fical Eletrônica informada, populando o banco de dados com as informações de itens, preços e taxas, emissores e pagamento.

## Documentação

Utilize a URL abaixo para ser direcionado para o Swagger da API

[Docs](http://nfceapi.herokuapp.com/swagger/index.html)

## Configuração

O projeto foi publicado utilizando recursos do Heroku (Dynos e Add-on). Não se torna obrigatório o uso dos mesmos, podendo substitui-los por quaisquer outros recursos de Deploy em Containers WEB e Conexão com banco de dados.

### Banco de dados
---

Utilize a URL abaixo para criar o banco de dados no seu SGBD favorito!

[Diagrama de Banco de Dados](https://dbdiagram.io/d/5ed16a8639d18f5553fff8c4)

Assim que o banco for criado, deve-se mudar o provedor (atual [ngpsql](https://www.npgsql.org/)) [aqui](Repositories/RepositoryBase.cs) e a ConnectionString [aqui](appsettings.json). O DommelEntityMap irá fazer o resto :) .

# Observações

* A API está apontando para a porta 5001 como Default. Caso queira mudar, modifique o arquivo [Program.cs](Program.cs)
