# Migration New Delivery;#5322
Delete ScenarioOrchestrator
# Tenant Provisioning - Ajout d'un mecanisme de FeatureTemplate de base;#5369
Rename SQL Table FeatureTemplate into Feature
Create SQL Structure for `FeatureTemplate`, `FeatureTemplateFeatures`
# Gestion multi-bannières du new-delivery
Modification SQL du modèle pour permettre le multi-bannières sur un User
Modification SQL du modèle pour permettre le multi-bannières sur une Signature (WithBanner)
Adaptation du DbContext DeliveryContext pour proprement permettre la many-to-many Banners/Users//Est-ce que ça ne remet pas sur la table les Bannières seules inexistantes
Adaptation du DbContext DeliveryContext pour permettre la many-to-many entre Signature et Banner// Uniquement pour scenario-by-signature
# Signature New builder;#5324
Manage signature spaces between firstname/lastname
# SynApps
Envoyer le mail de doc technique sur la sync par Filiales
// Commentaire ignoré
