#!/bin/bash

# Trolls et Paillettes - Unity Test Runner Script
# Run this script to execute all tests from terminal

echo "🎮 Trolls et Paillettes - Test Suite"
echo "======================================"
echo ""

# Check if Unity is running
if pgrep -x "Unity" > /dev/null; then
    echo "⚠️  ATTENTION: Unity est actuellement ouvert!"
    echo ""
    echo "Pour exécuter les tests depuis le terminal:"
    echo "1. Fermez Unity Editor complètement"
    echo "2. Relancez ce script"
    echo ""
    echo "OU"
    echo ""
    echo "Pour exécuter les tests dans Unity Editor:"
    echo "1. Dans Unity: Window → General → Test Runner"
    echo "2. Cliquez sur 'Run All'"
    echo ""
    exit 1
fi

echo "✅ Unity n'est pas en cours d'exécution"
echo "🚀 Démarrage des tests..."
echo ""

# Unity executable path
UNITY_PATH="/Applications/Unity/Hub/Editor/6000.2.6f1/Unity.app/Contents/MacOS/Unity"

# Project path
PROJECT_PATH="/Users/sergeychukhno/Desktop/CSharp/2D_unity_rpg/My project"

# Results path
RESULTS_PATH="/Users/sergeychukhno/Desktop/CSharp/2D_unity_rpg/TestResults.xml"
LOG_PATH="/Users/sergeychukhno/Desktop/CSharp/2D_unity_rpg/TestLog.txt"

# Run tests
"$UNITY_PATH" \
  -batchmode \
  -projectPath "$PROJECT_PATH" \
  -runTests \
  -testPlatform EditMode \
  -testResults "$RESULTS_PATH" \
  -logFile "$LOG_PATH"

# Check exit code
if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Tests exécutés avec succès!"
    echo ""
    echo "📊 Résultats disponibles dans:"
    echo "   - $RESULTS_PATH"
    echo "   - $LOG_PATH"
    echo ""
    
    # Parse and display results
    if [ -f "$RESULTS_PATH" ]; then
        echo "📈 Résumé des Tests:"
        grep -o 'total="[0-9]*"' "$RESULTS_PATH" | head -1
        grep -o 'passed="[0-9]*"' "$RESULTS_PATH" | head -1
        grep -o 'failed="[0-9]*"' "$RESULTS_PATH" | head -1
        echo ""
    fi
else
    echo ""
    echo "❌ Erreur lors de l'exécution des tests"
    echo "📝 Consultez le log: $LOG_PATH"
    echo ""
    exit 1
fi

