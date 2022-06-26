FROM node:latest as build
WORKDIR /app
COPY /src/QuickBudget.Web/. .
RUN npm install
RUN npm run build --configuration=$ENVIRONMENT

FROM nginx:alpine
COPY --from=build /app/dist/quickbudget_web /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

CMD sed -i -e 's/$PORT/'"$PORT"'/g' /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'
