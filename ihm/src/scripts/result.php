<?php

// Vérifier que la méthode HTTP utilisée est POST et que le champ "number" est défini
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['number'])) {
    // URL du service calc (Python)
    $url = 'http://calc:80/api/calcul';

    // Validation et assainissement de l'entrée utilisateur
    $number = filter_var($_POST['number'], FILTER_SANITIZE_NUMBER_FLOAT, FILTER_FLAG_ALLOW_FRACTION);

    // Préparer les données à envoyer au service
    $data = ['valeur' => $number];

    // Configuration de la requête HTTP
    $options = array(
        'http' => array(
            "header" => "Content-Type: application/json\r\n",
            "method" => "POST",
            "content" => json_encode($data),
            'timeout' => 5 // Timeout pour éviter les blocages
        )
    );

    // Créer le contexte pour la requête
    $context = stream_context_create($options);

    // Effectuer la requête et gérer les erreurs
    try {
        $response = file_get_contents($url, false, $context);

        // Vérifier si la réponse est valide
        if ($response === FALSE) {
            throw new Exception("Impossible de se connecter au service de calcul.");
        }

        // Décoder la réponse JSON
        $result = json_decode($response, true);

        // Vérifier si le JSON est valide
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw new Exception("La réponse du service n'est pas un JSON valide.");
        }

        // Vérifier si "result" existe avant d'y accéder
        $resultat_final = isset($result['result']) ? htmlspecialchars($result['result']) : "Aucun résultat disponible.";
    } catch (Exception $e) {
        // Gérer les erreurs et afficher un message utilisateur
        $error = "Erreur : " . htmlspecialchars($e->getMessage());
    }
}
