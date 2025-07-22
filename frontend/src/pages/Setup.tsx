import { InterviewSetupForm } from "@/components/InterviewSetupForm";

const Setup = () => {
  return (
    <div className="max-w-md mx-auto">
      <h1 className="text-3xl font-bold text-primary mb-8 text-center">
        Setup Your Interview
      </h1>
      <InterviewSetupForm />
    </div>
  );
};

export default Setup;