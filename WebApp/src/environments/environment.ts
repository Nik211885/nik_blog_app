export interface Environment {
  api: string;
  fe: string;
}

export const environment: Environment = {
    api: 'http://localhost:5132',
    fe: 'http://localhost:4200'
};
