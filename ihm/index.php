<? require_once 'scripts/result.php'; ?>

<!DOCTYPE html>
<html lang="fr">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>CalcuPHP#lation</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="style/style.css">
</head>

<body>
    <header>
        <h1>CalcuPHP#lation</h1>
    </header>
    <nav>
        <a href="#">Accueil</a>
        <a href="#">À propos</a>
        <a href="#">Services</a>
        <a href="#">Contact</a>
    </nav>
    <section>
        <h2>Bienvenue</h2>
        <p>CalcuPHP#lation est un outil de calcul en ligne. Il vous permet de réaliser des opérations simples et complexes.</p>
        <p>Veuillez saisir un nombre ci-dessous :</p>
        <form method="post">
            <input type="number" name="number" required>
            <button type="submit">Calculer</button>
        </form>
        <?php if (isset($isEven) && isset($isPerfect) && isset($isPrime) && isset($syracuse)) : ?>
            <div class=result>
                <h3>Résultats</h3>
                <ul>
                    <li>Le nombre est <?= $isEven ? 'pair' : 'impair' ?></li>
                    <li>Le nombre est <?= $isPerfect ? 'parfait' : 'imparfait' ?></li>
                    <li>Le nombre est <?= $isPrime ? 'premier' : 'non premier' ?></li>
                    <li>Suite de Syracuse : <?= $syracuse ?></li>
                </ul>
            </div>
        <?php endif; ?>

        <?php if (isset($error)) : ?>
            <p class="error"><?= $error ?></p>
        <?php endif; ?>
    </section>
    <footer>
        <p>© 2024 - Tous droits réservés</p>
    </footer>
</body>

</html>