from flask import Flask, render_template, request
from logic import validar_argumento

app = Flask(__name__)

@app.route('/', methods=['GET', 'POST'])
def index():
    resultado = None
    if request.method == 'POST':
        
        premisas_raw = request.form.get('premisas', '')
        conclusion = request.form.get('conclusion', '')

        if (not premisas_raw.strip()) or (not conclusion.strip()):
            resultado = {"error": "Por favor, ingresa al menos una premisa y una conclusion"}
            return render_template('index.html', resultado=resultado)
        
        else:
            lista_premisas = [p.strip() for p in premisas_raw.split('\n') if p.strip()]
            valido, tabla, variables, criticos = validar_argumento(lista_premisas, conclusion)
            
        
        if valido == "Error de sintaxis":
            resultado = {"error": "Hubo un error en la sintaxis de tus formulas. Revisa los operadores."}
        else:
            
            resultado = {
                'valido': valido,
                'tabla': tabla,
                'variables': variables,
                'premisas_nombres': lista_premisas,
                'conclusion_nombre': conclusion
            }

    
    return render_template('index.html', resultado=resultado)

if __name__ == '__main__':
    app.run(debug=True)