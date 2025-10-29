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
        "observacoes": null
      }
      """
    Then o payload deve obedecer ao schema "auditorias.schema.json"
    When eu faço POST em "/api/auditorias" com o payload
    Then o status deve ser 201
    And o corpo deve ser JSON válido
    And o corpo deve conter os campos "titulo, dataAgendada, auditorResponsavel, observacoes"

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
  Scenario: Criar auditoria e validar retorno no corpo
    Given o payload JSON:
      """
      {
        "titulo": "Auditoria Rastreavel",
        "dataAgendada": "2031-05-20",
        "auditorResponsavel": "Time A",
        "observacoes": "Em análise"
      }
      """
    When eu faço POST em "/api/auditorias" com o payload
    Then o status deve ser 201
    And o corpo deve ser JSON válido
    And o corpo deve conter os campos "titulo, dataAgendada, auditorResponsavel, observacoes"