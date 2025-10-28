Feature: Auditorias Ambientais
  Para garantir governança e conformidade,
  Como analista ESG,
  Quero validar criação e rastreabilidade de auditorias na API.

  Background:
    Given a base URL configurada

  @happy
  Scenario: Criar auditoria válida
    Given o payload JSON:
      """
      {
        "titulo": "Auditoria BDD",
        "dataAgendada": "2030-01-01",
        "auditorResponsavel": "Equipe ESG",
        "resultado": null
      }
      """
    Then o payload deve obedecer ao schema "auditorias.schema.json"
    When eu faço POST em "/api/auditorias" com o payload
    Then o status deve ser 201
    And o corpo deve ser JSON válido
    And o header "Location" deve existir

  @negative
  Scenario: Criar auditoria inválida (campos obrigatórios ausentes)
    Given o payload JSON:
      """
      { "titulo": "" }
      """
    When eu faço POST em "/api/auditorias" com o payload
    Then o status deve ser 400
    And o corpo deve ser JSON válido

  @traceability @governanca
  Scenario: Rastreabilidade - criar e recuperar auditoria pelo Location
    Given o payload JSON:
      """
      {
        "titulo": "Auditoria Rastreavel",
        "dataAgendada": "2031-05-20",
        "auditorResponsavel": "Time A",
        "resultado": "Em análise"
      }
      """
    When eu faço POST em "/api/auditorias" com o payload
    Then o status deve ser 201
    And eu extraio o "Location" como url do novo recurso
    When eu faço GET em "{url}"
    Then o status deve ser 200
    And o corpo deve ser JSON válido
    And o corpo deve conter os campos "titulo, dataAgendada, auditorResponsavel, resultado"