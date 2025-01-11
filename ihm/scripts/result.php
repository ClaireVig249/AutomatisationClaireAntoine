<?php

// Vérifier que la méthode HTTP utilisée est POST et que le champ "number" est défini
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['number'])) {
    // URL du service calc (Python)
    $url = 'http://localhost:8081';

    // Validation et assainissement de l'entrée utilisateur
    $number = filter_var($_POST['number'], FILTER_SANITIZE_NUMBER_FLOAT, FILTER_FLAG_ALLOW_FRACTION);

    // Préparer les données à envoyer au service
    $data = array('number' => $number);

    // Configuration de la requête HTTP
    $options = array(
        'http' => array(
            'header'  => "Content-type: application/x-www-form-urlencoded\r\n",
            'method'  => 'POST',
            'content' => http_build_query($data),
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

        // Afficher le résultat
        echo "Résultat : " . htmlspecialchars($result['result']);
    } catch (Exception $e) {
        // Gérer les erreurs et afficher un message utilisateur
        echo "Erreur : " . htmlspecialchars($e->getMessage());
    }
} else {
    // Si la requête n'est pas POST ou si le champ "number" est manquant
    echo "Veuillez soumettre un nombre via une requête POST.";
}
