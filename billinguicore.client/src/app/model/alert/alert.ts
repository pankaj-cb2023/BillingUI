export class Alert {
  type: string;
  description: string;
  sites: string;

  constructor(
    type: string,
    description: string,
    sites: string
  ) {
    this.type = type;
    this.description = description;
    this.sites = sites;
  }
}
