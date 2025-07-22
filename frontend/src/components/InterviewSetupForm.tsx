import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "@/components/ui/use-toast";

export const InterviewSetupForm = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    role: "",
    company: "",
    type: "technical",
    duration: "30",
    jobDescription: "",
    resume: null as File | null,
  });

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      // Check file type
      const fileType = file.type;
      const validTypes = [
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
      ];

      if (!validTypes.includes(fileType)) {
        toast({
          title: "Invalid File Type",
          description: "Please upload a PDF or Word document.",
          variant: "destructive",
        });
        return;
      }

      // Check file size (5MB limit)
      if (file.size > 5 * 1024 * 1024) {
        toast({
          title: "File Too Large",
          description: "Please upload a file smaller than 5MB.",
          variant: "destructive",
        });
        return;
      }

      setFormData({ ...formData, resume: file });
      toast({
        title: "File Uploaded",
        description: `${file.name} has been successfully uploaded.`,
      });
    }
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.role || !formData.company) {
      toast({
        title: "Missing Information",
        description: "Please fill in all required fields.",
        variant: "destructive",
      });
      return;
    }
    navigate("/preparation", { state: formData });
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-8 animate-fade-in">
      <div className="space-y-4">
        <div className="space-y-2">
          <Label htmlFor="role">Target Role</Label>
          <Input
            id="role"
            placeholder="e.g., Senior Software Engineer"
            value={formData.role}
            onChange={(e) => setFormData({ ...formData, role: e.target.value })}
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="company">Target Company</Label>
          <Input
            id="company"
            placeholder="e.g., Google"
            value={formData.company}
            onChange={(e) => setFormData({ ...formData, company: e.target.value })}
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="jobDescription">Job Description</Label>
          <Textarea
            id="jobDescription"
            placeholder="Paste the job description here..."
            value={formData.jobDescription}
            onChange={(e) =>
              setFormData({ ...formData, jobDescription: e.target.value })
            }
            className="min-h-[150px]"
          />
        </div>

        <div className="space-y-2">
          <Label htmlFor="resume">Resume/CV</Label>
          <Input
            id="resume"
            type="file"
            accept=".pdf,.doc,.docx,application/pdf,application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            onChange={handleFileChange}
            className="cursor-pointer"
          />
          <p className="text-sm text-muted-foreground">
            Upload your resume (PDF or Word, max 5MB)
          </p>
        </div>

        <div className="space-y-2">
          <Label>Interview Type</Label>
          <RadioGroup
            defaultValue="technical"
            onValueChange={(value) => setFormData({ ...formData, type: value })}
          >
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="technical" id="technical" />
              <Label htmlFor="technical">Technical</Label>
            </div>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="behavioral" id="behavioral" />
              <Label htmlFor="behavioral">Behavioral</Label>
            </div>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="product" id="product" />
              <Label htmlFor="product">Product Case Study</Label>
            </div>
          </RadioGroup>
        </div>

        <div className="space-y-2">
          <Label>Duration</Label>
          <RadioGroup
            defaultValue="30"
            onValueChange={(value) => setFormData({ ...formData, duration: value })}
          >
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="30" id="30min" />
              <Label htmlFor="30min">30 minutes</Label>
            </div>
            <div className="flex items-center space-x-2">
              <RadioGroupItem value="60" id="60min" />
              <Label htmlFor="60min">1 hour</Label>
            </div>
          </RadioGroup>
        </div>
      </div>

      <Button type="submit" className="w-full">
        Start Preparation
      </Button>
    </form>
  );
};