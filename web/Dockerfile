FROM node:18-alpine as build
WORKDIR /app
COPY package*.json ./
COPY src/theme ./src/theme
RUN mkdir static
RUN npm ci --cache .npm
COPY . .
RUN npm run build

FROM node:18-alpine AS runtime
WORKDIR /app
COPY --from=build /app/build .
COPY --from=build /app/package.json .
COPY --from=build /app/package-lock.json .
RUN npm ci --ignore-scripts --omit dev
EXPOSE 3000
CMD ["node", "."]