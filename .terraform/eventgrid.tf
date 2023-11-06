resource "azurerm_eventgrid_event_subscription" "eg_subscription_to_api" {
  name                  = "my-event-subscription-to-api"
  scope                 = azurerm_storage_account.sa.id
  included_event_types  = ["Microsoft.Storage.BlobCreated"]
  
  webhook_endpoint {
    url = "https://yourapiendpoint.com/eventgridhook"
  }

  # Retentativa e Dead Lettering
  retry_policy {
    max_delivery_attempts = 5
    event_time_to_live    = "PT2H" # Evento expira após 2 horas
  }

  dead_letter_destination {
    storage_blob_container_destination {
      resource_id       = azurerm_storage_account.sa.id
      blob_container_name = "deadletters"
    }
  }

  # Logs
  storage_blob_dead_letter_destination {
    storage_account_id = azurerm_storage_account.sa.id
  }
}

resource "azurerm_storage_container" "deadletters" {
  name                  = "deadletters"
  storage_account_name  = azurerm_storage_account.sa.name
  container_access_type = "private"
}
