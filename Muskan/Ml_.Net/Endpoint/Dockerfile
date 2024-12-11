
FROM python:3.10.6

WORKDIR /app

COPY requirements.txt /app/

RUN pip install --no-cache-dir -r requirements.txt


COPY . /app


EXPOSE 5000


CMD ["python", "main.py"]
