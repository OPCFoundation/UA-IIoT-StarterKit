export enum UserLoginStatus {
   Unknown = -1,
   LoggedOut = 0,
   InProgress = 1,
   LoggedIn = 2
}

export enum ReleaseStatus {
   Invalid = 0,
   Draft = 1,
   ReleaseCandidate = 2,
   Released = 3,
   Deprecated = 4
}

export interface ErrorInfo {
   errorCode?: string
   errorText?: string
   silent?: boolean
   failed?: boolean
}

export interface BaseRecord {
   id?: number
}

export interface NamedRecord extends BaseRecord {
   name?: string
}

export enum MembershipType {
   NonMember = 0,
   Logo = 1,
   NonVoting = 2,
   EndUser = 3,
   Corporate = 4
}

export interface Account extends NamedRecord {
   email?: string
   gitHubId?: string
   companyName?: string
   membershipType?: MembershipType,
   currentGitHubUser?: string
   accessToken?: string
}
