# Web Api - Imobilizados

## Conteúdo
Através da tecnologia REST utilizamos artificios para expor formas de acesso aos dados armazenados de gerenciamento de imobilizados. Estes dados são persistidos via MongoDB e apresentados via Web API .Net Core.

Todas as informações são previamente testadas com a ferramenta xUnit.net em conjunto com o Moq.

## Recursos

As ações são executadas a partir de rotas mapeadas para verbos HTTP especificos, são eles:

* POST /api/hardware - a partir deste recurso é criado um novo equipamento
* PUT /api/hardware/{id} a partir deste recurso é atualizada informações de um equipamento especifico
* GET /api/hardware/{id} a partir deste recurso é detalhado um equipamento especifico
* GET /api/hardware/all a partir deste recurso é listado todos os equipamentos armazenados
* PUT /api/hardware/immobilize/{id} a partir deste recurso é atualizado informação em qual andar o equipamento está imobilizado
* GET /api/hardware/immobilized?is_immobilized=false a partir deste recurso é listado os equipamentos imobilizados ou nao
* GET /api/hardware/immobilized/floor/{floor} a partir deste recurso é listado equipamentos imobilizados em um andar

